using System.Data;
using MySqlConnector;

namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Kullanıcı oluşturma, giriş ve listeleme — formlardaki tekrarlayan SQL'i tek yerde toplar.
    /// </summary>
    internal static class KullaniciServisi
    {
        public static IslemSonucu Olustur(
            string ad,
            string soyad,
            string kullaniciAdi,
            string sifre,
            string sifreTekrar,
            string rolAdi,
            string? izinliRolSiniriMesaji = null,
            params string[] izinliRoller)
        {
            ad = ad.Trim();
            soyad = soyad.Trim();
            kullaniciAdi = kullaniciAdi.Trim();

            if (string.IsNullOrWhiteSpace(ad) ||
                string.IsNullOrWhiteSpace(soyad) ||
                string.IsNullOrWhiteSpace(kullaniciAdi) ||
                string.IsNullOrWhiteSpace(sifre) ||
                string.IsNullOrWhiteSpace(sifreTekrar))
            {
                return IslemSonucu.Hata("Lütfen tüm alanları doldurunuz.");
            }

            if (sifre != sifreTekrar)
            {
                return IslemSonucu.Hata("Şifreler birbiriyle eşleşmiyor.");
            }

            if (string.Equals(kullaniciAdi, Veritabani.AdminKullaniciAdi, StringComparison.OrdinalIgnoreCase))
            {
                return IslemSonucu.Hata("Bu kullanıcı adı sistem tarafından ayrılmıştır.");
            }

            if (izinliRoller.Length > 0 &&
                !izinliRoller.Any(r => string.Equals(r, rolAdi, StringComparison.OrdinalIgnoreCase)))
            {
                return IslemSonucu.Hata(izinliRolSiniriMesaji ?? "Bu rol için hesap oluşturulamaz.");
            }

            if (Veritabani.Sayi(
                    "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi = @kullaniciAdi",
                    ("@kullaniciAdi", kullaniciAdi)) > 0)
            {
                return IslemSonucu.Hata("Bu kullanıcı adı zaten kayıtlı.");
            }

            try
            {
                Veritabani.Calistir(
                    @"INSERT INTO Kullanicilar (Ad, Soyad, KullaniciAdi, Sifre, RolAdi)
                      VALUES (@ad, @soyad, @kullaniciAdi, @sifre, @rolAdi)",
                    ("@ad", ad),
                    ("@soyad", soyad),
                    ("@kullaniciAdi", kullaniciAdi),
                    ("@sifre", sifre),
                    ("@rolAdi", rolAdi));
                return IslemSonucu.Ok();
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                return IslemSonucu.Hata("Bu kullanıcı adı zaten kayıtlı.");
            }
        }

        public static IslemSonucu<OturumBilgisi> GirisYap(string kullaniciAdi, string sifre)
        {
            if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
            {
                return IslemSonucu<OturumBilgisi>.Hata("Kullanıcı adı ve şifre boş bırakılamaz.");
            }

            DataTable tablo = Veritabani.TabloGetir(
                @"SELECT KullaniciID, Ad, Soyad, RolAdi, Durum
                  FROM Kullanicilar
                  WHERE KullaniciAdi = @kullaniciAdi AND Sifre = @sifre
                  LIMIT 1",
                ("@kullaniciAdi", kullaniciAdi.Trim()),
                ("@sifre", sifre));

            if (tablo.Rows.Count == 0)
            {
                return IslemSonucu<OturumBilgisi>.Hata("Kullanıcı adı veya şifre hatalı.");
            }

            DataRow satir = tablo.Rows[0];
            string durum = satir["Durum"] == DBNull.Value
                ? Veritabani.DurumAktif
                : Convert.ToString(satir["Durum"]) ?? Veritabani.DurumAktif;

            if (string.Equals(durum, Veritabani.DurumPasif, StringComparison.OrdinalIgnoreCase))
            {
                return IslemSonucu<OturumBilgisi>.Hata("Kullanıcı hesabı pasif. Sisteme girişe izin verilmez.");
            }

            OturumBilgisi oturum = new(
                Convert.ToInt32(satir["KullaniciID"]),
                $"{satir["Ad"]} {satir["Soyad"]}",
                Convert.ToString(satir["RolAdi"]) ?? string.Empty);

            return IslemSonucu<OturumBilgisi>.Ok(oturum);
        }

        public static DataTable RolleListele(params string[] roller)
        {
            if (roller.Length == 0)
            {
                return new DataTable();
            }

            if (roller.Length == 1)
            {
                return Veritabani.TabloGetir(
                    @"SELECT KullaniciID, Ad, Soyad, KullaniciAdi, RolAdi
                      FROM Kullanicilar
                      WHERE RolAdi = @rol
                      ORDER BY KullaniciID DESC",
                    ("@rol", roller[0]));
            }

            return Veritabani.TabloGetir(
                @"SELECT KullaniciID, Ad, Soyad, KullaniciAdi, RolAdi
                  FROM Kullanicilar
                  WHERE RolAdi IN (@rol1, @rol2)
                  ORDER BY RolAdi, KullaniciID DESC",
                ("@rol1", roller[0]),
                ("@rol2", roller[1]));
        }

        public static void KullaniciGridBasliklari(DataGridView grid)
        {
            UiTema.KolonBasliklari(grid,
                ("KullaniciID", "ID"),
                ("KullaniciAdi", "Kullanıcı Adı"),
                ("RolAdi", "Rol"));
        }
    }

    internal sealed record OturumBilgisi(int KullaniciId, string AdSoyad, string RolAdi);
}
