using MySqlConnector;

namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Aidat dönemi / borç oluşturma ve ödeme kaydı (transaction ile).
    /// </summary>
    internal static class AidatServisi
    {
        public static IslemSonucu AidatOlustur(
            string donem,
            decimal tutar,
            DateTime sonOdeme,
            int olusturanKullaniciId,
            IReadOnlyList<int> daireIdleri)
        {
            if (tutar <= 0)
            {
                return IslemSonucu.Hata("Tutar geçersiz. Sıfırdan büyük bir tutar giriniz.");
            }

            if (daireIdleri.Count == 0)
            {
                return IslemSonucu.Hata("En az bir daire seçiniz.");
            }

            int eklenen = 0;
            int atlanan = 0;

            Veritabani.IslemIcinde((baglanti, tx) =>
            {
                long donemId = DonemIdAlVeyaOlustur(baglanti, tx, donem, tutar, sonOdeme, olusturanKullaniciId);

                foreach (int daireId in daireIdleri)
                {
                    using MySqlCommand kontrol = Veritabani.YeniKomut(
                        baglanti,
                        tx,
                        "SELECT COUNT(*) FROM AidatBorclari WHERE AidatDonemID = @donem AND DaireID = @daire",
                        ("@donem", donemId),
                        ("@daire", daireId));

                    if (Convert.ToInt32(kontrol.ExecuteScalar()) > 0)
                    {
                        atlanan++;
                        continue;
                    }

                    using MySqlCommand borc = Veritabani.YeniKomut(
                        baglanti,
                        tx,
                        @"INSERT INTO AidatBorclari (AidatDonemID, DaireID, Tutar, KalanTutar, Durum)
                          VALUES (@donem, @daire, @tutar, @tutar, @durum)",
                        ("@donem", donemId),
                        ("@daire", daireId),
                        ("@tutar", tutar),
                        ("@durum", Veritabani.BorcOdenmedi));
                    borc.ExecuteNonQuery();
                    eklenen++;
                }
            });

            string mesaj = $"{eklenen} daire için aidat borcu oluşturuldu.";
            if (atlanan > 0)
            {
                mesaj += $" {atlanan} dairede bu dönem zaten kayıtlıydı, mükerrer yazılmadı.";
            }

            return IslemSonucu.Ok(mesaj);
        }

        public static IslemSonucu OdemeKaydet(
            int aidatBorcId,
            decimal odemeTutari,
            decimal mevcutKalan,
            decimal toplamTutar,
            DateTime odemeTarihi,
            string odemeYontemi,
            int kaydedenKullaniciId)
        {
            if (odemeTutari <= 0)
            {
                return IslemSonucu.Hata("Ödeme tutarı geçersiz.");
            }

            if (mevcutKalan <= 0)
            {
                return IslemSonucu.Hata("Bu borcun kalan tutarı yok.");
            }

            decimal odeme = Math.Min(odemeTutari, mevcutKalan);
            decimal yeniKalan = mevcutKalan - odeme;

            Veritabani.IslemIcinde((baglanti, tx) =>
            {
                using MySqlCommand ekle = Veritabani.YeniKomut(
                    baglanti,
                    tx,
                    @"INSERT INTO Odemeler (AidatBorcID, Tutar, OdemeTarihi, OdemeYontemi, KaydedenKullaniciID)
                      VALUES (@borc, @tutar, @tarih, @yontem, @kullanici)",
                    ("@borc", aidatBorcId),
                    ("@tutar", odeme),
                    ("@tarih", odemeTarihi.Date),
                    ("@yontem", odemeYontemi),
                    ("@kullanici", kaydedenKullaniciId));
                ekle.ExecuteNonQuery();

                using MySqlCommand guncelle = Veritabani.YeniKomut(
                    baglanti,
                    tx,
                    "UPDATE AidatBorclari SET KalanTutar = @kalan, Durum = @durum WHERE AidatBorcID = @id",
                    ("@kalan", yeniKalan),
                    ("@durum", Veritabani.BorcDurumu(yeniKalan, toplamTutar)),
                    ("@id", aidatBorcId));
                guncelle.ExecuteNonQuery();
            });

            return IslemSonucu.Ok("Ödeme kaydedildi, kalan borç güncellendi.");
        }

        private static long DonemIdAlVeyaOlustur(
            MySqlConnection baglanti,
            MySqlTransaction tx,
            string donem,
            decimal tutar,
            DateTime sonOdeme,
            int olusturanKullaniciId)
        {
            using (MySqlCommand ara = Veritabani.YeniKomut(
                       baglanti,
                       tx,
                       "SELECT AidatDonemID FROM AidatDonemleri WHERE Donem = @donem",
                       ("@donem", donem)))
            {
                object? id = ara.ExecuteScalar();
                if (id != null)
                {
                    return Convert.ToInt64(id);
                }
            }

            using MySqlCommand ekle = Veritabani.YeniKomut(
                baglanti,
                tx,
                @"INSERT INTO AidatDonemleri (Donem, Tutar, SonOdemeTarihi, OlusturanKullaniciID)
                  VALUES (@donem, @tutar, @son, @kullanici)",
                ("@donem", donem),
                ("@tutar", tutar),
                ("@son", sonOdeme.Date),
                ("@kullanici", olusturanKullaniciId));
            ekle.ExecuteNonQuery();
            return ekle.LastInsertedId;
        }
    }
}
