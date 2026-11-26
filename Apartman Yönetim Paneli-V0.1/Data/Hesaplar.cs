using System.Data;

namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Giriş ve kullanıcı kaydı. Üç formdaki aynı sorgular burada tek yerde durur.
    /// </summary>
    internal static class Hesaplar
    {
        public static GirisKaydi? GirisYap(string kullaniciAdi, string sifre)
        {
            DataTable tablo = Veritabani.TabloGetir(
                @"SELECT KullaniciID, Ad, Soyad, RolAdi, Durum, Sifre
                  FROM Kullanicilar
                  WHERE KullaniciAdi = @kullaniciAdi
                  LIMIT 1",
                ("@kullaniciAdi", kullaniciAdi));
            if (tablo.Rows.Count == 0)
            {
                return null;
            }

            DataRow satir = tablo.Rows[0];
            string kayitli = Convert.ToString(satir["Sifre"]) ?? "";
            if (!SifreKoruma.Dogrula(sifre, kayitli))
            {
                return null;
            }

            string durum = satir["Durum"] == DBNull.Value
                ? Veritabani.DurumAktif
                : Convert.ToString(satir["Durum"]) ?? Veritabani.DurumAktif;
            return new GirisKaydi(
                Convert.ToInt32(satir["KullaniciID"]),
                $"{satir["Ad"]} {satir["Soyad"]}",
                Convert.ToString(satir["RolAdi"]) ?? "",
                durum);
        }

        public static bool KullaniciVar(string kullaniciAdi)
        {
            return Veritabani.Sayi(
                "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi = @kullaniciAdi",
                ("@kullaniciAdi", kullaniciAdi)) > 0;
        }

        public static void KullaniciEkle(string ad, string soyad, string kullaniciAdi, string sifre, string rolAdi)
        {
            Veritabani.Calistir(
                @"INSERT INTO Kullanicilar (Ad, Soyad, KullaniciAdi, Sifre, RolAdi)
                  VALUES (@ad, @soyad, @kullaniciAdi, @sifre, @rolAdi)",
                ("@ad", ad),
                ("@soyad", soyad),
                ("@kullaniciAdi", kullaniciAdi),
                ("@sifre", SifreKoruma.Ozetle(sifre)),
                ("@rolAdi", rolAdi));
        }

        public static DataTable Yoneticiler()
        {
            return Veritabani.TabloGetir(
                @"SELECT KullaniciID, Ad, Soyad, KullaniciAdi, RolAdi
                  FROM Kullanicilar
                  WHERE RolAdi = @rolAdi
                  ORDER BY KullaniciID DESC",
                ("@rolAdi", Veritabani.RolYonetici));
        }

        public static DataTable Personeller()
        {
            return Veritabani.TabloGetir(
                @"SELECT KullaniciID, Ad, Soyad, KullaniciAdi, RolAdi
                  FROM Kullanicilar
                  WHERE RolAdi IN (@muhasebeci, @teknisyen, @guvenlik)
                  ORDER BY RolAdi, KullaniciID DESC",
                ("@muhasebeci", Veritabani.RolMuhasebeci),
                ("@teknisyen", Veritabani.RolTeknisyen),
                ("@guvenlik", Veritabani.RolGuvenlik));
        }
    }

    internal sealed class GirisKaydi
    {
        public GirisKaydi(int kullaniciId, string adSoyad, string rolAdi, string durum)
        {
            KullaniciId = kullaniciId;
            AdSoyad = adSoyad;
            RolAdi = rolAdi;
            Durum = durum;
        }

        public int KullaniciId { get; }
        public string AdSoyad { get; }
        public string RolAdi { get; }
        public string Durum { get; }
    }
}
