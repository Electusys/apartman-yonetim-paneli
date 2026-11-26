using MySqlConnector;

namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Kargo, daire bildirimi ve güvenlik günlüğünü tek işlemde yazar.
    /// </summary>
    internal static class KargoKaydi
    {
        public static void Ekle(int daireId, string bilgi, DateTime teslim, int kullaniciId)
        {
            using MySqlConnection baglanti = Veritabani.BaglantiAc();
            using MySqlTransaction islem = baglanti.BeginTransaction();
            long kargoId;
            using (MySqlCommand ekle = new MySqlCommand(
                @"INSERT INTO Kargolar (DaireID, KargoBilgisi, TeslimAlmaZamani, KaydedenKullaniciID)
                  VALUES (@daire, @bilgi, @zaman, @kullanici)", baglanti, islem))
            {
                ekle.Parameters.AddWithValue("@daire", daireId);
                ekle.Parameters.AddWithValue("@bilgi", bilgi);
                ekle.Parameters.AddWithValue("@zaman", teslim);
                ekle.Parameters.AddWithValue("@kullanici", kullaniciId);
                ekle.ExecuteNonQuery();
                kargoId = ekle.LastInsertedId;
            }

            using (MySqlCommand bildirim = new MySqlCommand(
                @"INSERT INTO DaireBildirimleri
                    (DaireID, Baslik, Icerik, KargoID, OlusturanKullaniciID)
                  VALUES (@daire, @baslik, @icerik, @kargo, @kullanici)", baglanti, islem))
            {
                bildirim.Parameters.AddWithValue("@daire", daireId);
                bildirim.Parameters.AddWithValue("@baslik", "Kargo teslim alındı");
                bildirim.Parameters.AddWithValue("@icerik", "Güvenlik bir kargo teslim aldı: " + bilgi);
                bildirim.Parameters.AddWithValue("@kargo", kargoId);
                bildirim.Parameters.AddWithValue("@kullanici", kullaniciId);
                bildirim.ExecuteNonQuery();
            }

            using (MySqlCommand gunluk = new MySqlCommand(
                @"INSERT INTO GuvenlikIslemKayitlari (KullaniciID, IslemTuru, Aciklama)
                  VALUES (@id, @tur, @aciklama)", baglanti, islem))
            {
                gunluk.Parameters.AddWithValue("@id", kullaniciId);
                gunluk.Parameters.AddWithValue("@tur", "Kargo bildirimi");
                gunluk.Parameters.AddWithValue("@aciklama", $"Kargo kaydı ve daire bildirimi oluşturuldu. {bilgi}");
                gunluk.ExecuteNonQuery();
            }

            islem.Commit();
        }
    }
}
