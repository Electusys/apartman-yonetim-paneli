using MySqlConnector;

namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Aidat dönemi ve daire borçlarını tek işlemde yazar.
    /// </summary>
    internal static class AidatKaydi
    {
        public static (int Eklenen, int Atlanan) BorcOlustur(
            string donem,
            decimal tutar,
            DateTime sonOdeme,
            int kullaniciId,
            IReadOnlyList<int> daireler)
        {
            int eklenen = 0;
            int atlanan = 0;
            using MySqlConnection baglanti = Veritabani.BaglantiAc();
            using MySqlTransaction tx = baglanti.BeginTransaction();

            long donemId;
            using (MySqlCommand ara = new MySqlCommand("SELECT AidatDonemID FROM AidatDonemleri WHERE Donem = @donem", baglanti, tx))
            {
                ara.Parameters.AddWithValue("@donem", donem);
                object? id = ara.ExecuteScalar();
                if (id == null)
                {
                    using MySqlCommand ekle = new MySqlCommand(
                        @"INSERT INTO AidatDonemleri (Donem, Tutar, SonOdemeTarihi, OlusturanKullaniciID)
                          VALUES (@donem, @tutar, @son, @kullanici)", baglanti, tx);
                    ekle.Parameters.AddWithValue("@donem", donem);
                    ekle.Parameters.AddWithValue("@tutar", tutar);
                    ekle.Parameters.AddWithValue("@son", sonOdeme);
                    ekle.Parameters.AddWithValue("@kullanici", kullaniciId);
                    ekle.ExecuteNonQuery();
                    donemId = ekle.LastInsertedId;
                }
                else
                {
                    donemId = Convert.ToInt64(id);
                }
            }

            foreach (int daireId in daireler)
            {
                using MySqlCommand kontrol = new MySqlCommand(
                    "SELECT COUNT(*) FROM AidatBorclari WHERE AidatDonemID = @donem AND DaireID = @daire", baglanti, tx);
                kontrol.Parameters.AddWithValue("@donem", donemId);
                kontrol.Parameters.AddWithValue("@daire", daireId);
                if (Convert.ToInt32(kontrol.ExecuteScalar()) > 0)
                {
                    atlanan++;
                    continue;
                }

                using MySqlCommand borc = new MySqlCommand(
                    @"INSERT INTO AidatBorclari (AidatDonemID, DaireID, Tutar, KalanTutar, Durum)
                      VALUES (@donem, @daire, @tutar, @tutar, @durum)", baglanti, tx);
                borc.Parameters.AddWithValue("@donem", donemId);
                borc.Parameters.AddWithValue("@daire", daireId);
                borc.Parameters.AddWithValue("@tutar", tutar);
                borc.Parameters.AddWithValue("@durum", Veritabani.BorcOdenmedi);
                borc.ExecuteNonQuery();
                eklenen++;
            }

            tx.Commit();
            return (eklenen, atlanan);
        }
    }
}
