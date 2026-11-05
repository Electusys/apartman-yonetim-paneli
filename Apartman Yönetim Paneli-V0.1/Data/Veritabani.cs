using System.Configuration;
using System.Data;
using MySqlConnector;

namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// MySQL bağlantı bilgileri, rol adları ve ilk kurulum işlemleri.
    /// Bağlantı ve Admin bilgileri App.config üzerinden okunur.
    /// </summary>
    internal static class Veritabani
    {
        // Yerel demo varsayılanı. Gerçek şifreyi App.config içinden değiştirin.
        private const string VarsayilanBaglanti =
            "Server=127.0.0.1;Port=3306;Database=ApartmanYonetimDB;Uid=root;Pwd=;";

        public static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["ApartmanDb"]?.ConnectionString
            ?? VarsayilanBaglanti;

        // Sistem yöneticisi hesabı: kayıt formundan alınamaz, yalnızca bu sınıf oluşturur/günceller.
        public static string AdminKullaniciAdi =>
            ConfigurationManager.AppSettings["AdminKullaniciAdi"] ?? "Admin";

        public static string AdminSifre =>
            ConfigurationManager.AppSettings["AdminSifre"] ?? "Admin123";

        // Roller hiyerarşisi:
        // Admin → apartman yöneticisi oluşturur
        // Apartman Yöneticisi → muhasebeci ve teknisyen atar
        // Diğerleri kendi panellerine girer
        public const string RolAdmin = "Admin";
        public const string RolYonetici = "Apartman Yöneticisi";
        public const string RolMuhasebeci = "Muhasebeci";
        public const string RolTeknisyen = "Teknisyen";
        public const string RolSakin = "Apartman Sakini";
        public const string DurumAktif = "Aktif";
        public const string DurumPasif = "Pasif";
        public const string ArizaBeklemede = "Beklemede";
        public const string ArizaIslemde = "İşlem Yapılıyor";
        public const string ArizaTamamlandi = "Tamamlandı";
        public const string BorcOdenmedi = "Odenmedi";
        public const string BorcKismi = "KismiOdendi";
        public const string BorcOdendi = "Odendi";

        public static MySqlConnection BaglantiAc()
        {
            MySqlConnection baglanti = new MySqlConnection(ConnectionString);
            baglanti.Open();
            return baglanti;
        }

        public static DataTable TabloGetir(string sql, params (string Ad, object? Deger)[] parametreler)
        {
            using MySqlConnection baglanti = BaglantiAc();
            using MySqlCommand komut = Komut(baglanti, sql, parametreler);
            using MySqlDataAdapter adaptor = new MySqlDataAdapter(komut);
            DataTable tablo = new DataTable();
            adaptor.Fill(tablo);
            return tablo;
        }

        public static object? Skaler(string sql, params (string Ad, object? Deger)[] parametreler)
        {
            using MySqlConnection baglanti = BaglantiAc();
            using MySqlCommand komut = Komut(baglanti, sql, parametreler);
            return komut.ExecuteScalar();
        }

        public static int Calistir(string sql, params (string Ad, object? Deger)[] parametreler)
        {
            using MySqlConnection baglanti = BaglantiAc();
            using MySqlCommand komut = Komut(baglanti, sql, parametreler);
            return komut.ExecuteNonQuery();
        }

        public static long EkleVeId(string sql, params (string Ad, object? Deger)[] parametreler)
        {
            using MySqlConnection baglanti = BaglantiAc();
            using MySqlCommand komut = Komut(baglanti, sql, parametreler);
            komut.ExecuteNonQuery();
            return komut.LastInsertedId;
        }

        public static int Sayi(string sql, params (string Ad, object? Deger)[] parametreler)
        {
            return Convert.ToInt32(Skaler(sql, parametreler) ?? 0);
        }

        private static MySqlCommand Komut(MySqlConnection baglanti, string sql, (string Ad, object? Deger)[] parametreler)
        {
            MySqlCommand komut = new MySqlCommand(sql, baglanti);
            foreach ((string ad, object? deger) in parametreler)
            {
                komut.Parameters.AddWithValue(ad, deger ?? DBNull.Value);
            }

            return komut;
        }

        public static List<ListeKalemi> DaireListesi()
        {
            DataTable tablo = TabloGetir(
                @"SELECT DaireID, CONCAT(IFNULL(Blok, '-'), ' / ', DaireNo) AS Etiket
                  FROM Daireler
                  ORDER BY Blok, DaireNo");
            List<ListeKalemi> liste = new List<ListeKalemi>();
            foreach (DataRow satir in tablo.Rows)
            {
                liste.Add(new ListeKalemi(Convert.ToInt32(satir["DaireID"]), Convert.ToString(satir["Etiket"]) ?? ""));
            }

            return liste;
        }

        public static List<ListeKalemi> KullaniciDaireleri(int kullaniciId)
        {
            DataTable tablo = TabloGetir(
                @"SELECT DISTINCT DaireID, Etiket FROM (
                      SELECT d.DaireID, CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Etiket
                      FROM Sakinler s
                      INNER JOIN Daireler d ON d.DaireID = s.DaireID
                      WHERE s.KullaniciID = @id AND s.Aktif = 1
                      UNION
                      SELECT d.DaireID, CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Etiket
                      FROM Daireler d
                      WHERE d.KullaniciID = @id
                  ) t
                  ORDER BY Etiket",
                ("@id", kullaniciId));
            List<ListeKalemi> liste = new List<ListeKalemi>();
            foreach (DataRow satir in tablo.Rows)
            {
                liste.Add(new ListeKalemi(Convert.ToInt32(satir["DaireID"]), Convert.ToString(satir["Etiket"]) ?? ""));
            }

            return liste;
        }

        public static string BorcDurumu(decimal kalan, decimal tutar)
        {
            if (kalan <= 0)
            {
                return BorcOdendi;
            }

            if (kalan < tutar)
            {
                return BorcKismi;
            }

            return BorcOdenmedi;
        }

        /// <summary>
        /// Giriş ekranı açılırken çağrılır.
        /// Rolleri eksikse ekler, Admin kullanıcısı yoksa oluşturur, varsa şifre/rolünü senkronlar.
        /// Böylece veritabanı sıfırlansa bile Admin hesabı her açılışta hazır olur.
        /// </summary>
        public static void AdminHesabiniHazirla()
        {
            using MySqlConnection baglanti = new MySqlConnection(ConnectionString);
            baglanti.Open();

            // Önce rol sözlüğünü doldur; kullanıcı kaydı RolAdi metnine bağlıdır.
            RoluEkle(baglanti, RolAdmin);
            RoluEkle(baglanti, RolYonetici);
            RoluEkle(baglanti, RolMuhasebeci);
            RoluEkle(baglanti, RolTeknisyen);
            RoluEkle(baglanti, RolSakin);

            // Admin kullanıcı adı daha önce kaydedilmiş mi?
            using MySqlCommand kontrol = new MySqlCommand(
                "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi = @kullaniciAdi", baglanti);
            kontrol.Parameters.AddWithValue("@kullaniciAdi", AdminKullaniciAdi);
            int kayitSayisi = Convert.ToInt32(kontrol.ExecuteScalar());

            if (kayitSayisi == 0)
            {
                // İlk kurulum: sabit Admin hesabını ekle.
                using MySqlCommand ekle = new MySqlCommand(
                    @"INSERT INTO Kullanicilar (Ad, Soyad, KullaniciAdi, Sifre, RolAdi)
                      VALUES (@ad, @soyad, @kullaniciAdi, @sifre, @rolAdi)", baglanti);
                ekle.Parameters.AddWithValue("@ad", "Sistem");
                ekle.Parameters.AddWithValue("@soyad", "Yöneticisi");
                ekle.Parameters.AddWithValue("@kullaniciAdi", AdminKullaniciAdi);
                ekle.Parameters.AddWithValue("@sifre", AdminSifre);
                ekle.Parameters.AddWithValue("@rolAdi", RolAdmin);
                ekle.ExecuteNonQuery();
                return;
            }

            // Hesap varsa şifre veya rol elle değişmiş olsa bile koddaki değerlere geri çek.
            using MySqlCommand guncelle = new MySqlCommand(
                @"UPDATE Kullanicilar
                  SET Sifre = @sifre, RolAdi = @rolAdi, Ad = @ad, Soyad = @soyad
                  WHERE KullaniciAdi = @kullaniciAdi", baglanti);
            guncelle.Parameters.AddWithValue("@sifre", AdminSifre);
            guncelle.Parameters.AddWithValue("@rolAdi", RolAdmin);
            guncelle.Parameters.AddWithValue("@ad", "Sistem");
            guncelle.Parameters.AddWithValue("@soyad", "Yöneticisi");
            guncelle.Parameters.AddWithValue("@kullaniciAdi", AdminKullaniciAdi);
            guncelle.ExecuteNonQuery();
        }

        /// <summary>
        /// Roller tablosuna aynı adı iki kez yazmamak için önce COUNT bakılır.
        /// Tablo yoksa hata yutulur; kullanıcı kaydı yine de RolAdi sütunuyla çalışır.
        /// </summary>
        private static void RoluEkle(MySqlConnection baglanti, string rolAdi)
        {
            try
            {
                using MySqlCommand kontrol = new MySqlCommand(
                    "SELECT COUNT(*) FROM Roller WHERE RolAdi = @rolAdi", baglanti);
                kontrol.Parameters.AddWithValue("@rolAdi", rolAdi);
                int sayi = Convert.ToInt32(kontrol.ExecuteScalar());
                if (sayi > 0)
                {
                    return;
                }

                using MySqlCommand ekle = new MySqlCommand(
                    "INSERT INTO Roller (RolAdi) VALUES (@rolAdi)", baglanti);
                ekle.Parameters.AddWithValue("@rolAdi", rolAdi);
                ekle.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                // Roller tablosu yoksa kullanıcı kaydı yine de RolAdi ile devam eder.
            }
        }
    }

    /// <summary>
    /// ComboBox / CheckedListBox satırı: ekranda metin, veritabanında Id.
    /// </summary>
    internal sealed class ListeKalemi
    {
        public int Id { get; }
        public string Metin { get; }

        public ListeKalemi(int id, string metin)
        {
            Id = id;
            Metin = metin;
        }

        public override string ToString() => Metin;
    }
}
