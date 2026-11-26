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
        public static string ConnectionString
        {
            get
            {
                string? deger = ConfigurationManager.ConnectionStrings["ApartmanDb"]?.ConnectionString;
                if (string.IsNullOrWhiteSpace(deger))
                {
                    throw new InvalidOperationException("App.config içinde ApartmanDb bağlantısı yok.");
                }

                return deger;
            }
        }

        // Sistem yöneticisi hesabı: kayıt formundan alınamaz, yalnızca bu sınıf oluşturur/günceller.
        public static string AdminKullaniciAdi =>
            ConfigurationManager.AppSettings["AdminKullaniciAdi"] ?? "admin";

        public static string AdminSifre =>
            ConfigurationManager.AppSettings["AdminSifre"] ?? "1234";

        // Roller hiyerarşisi:
        // Admin → apartman yöneticisi oluşturur
        // Apartman Yöneticisi → muhasebeci, teknisyen ve güvenlik atar
        // Diğerleri kendi panellerine girer
        public const string RolAdmin = "Admin";
        public const string RolYonetici = "Apartman Yöneticisi";
        public const string RolMuhasebeci = "Muhasebeci";
        public const string RolTeknisyen = "Teknisyen";
        public const string RolGuvenlik = "Güvenlik";
        public const string RolSakin = "Apartman Sakini";
        public const string GuvenlikKullaniciAdi = "güvenlik";
        public const string GuvenlikSifre = "1234";
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
        /// Rolleri eksikse ekler. Admin ve güvenlik hesabı yoksa bir kez oluşturur.
        /// Var olan şifreyi açılışta değiştirmez. Düz metin şifreleri özete çevirir.
        /// </summary>
        public static void AdminHesabiniHazirla()
        {
            using MySqlConnection baglanti = new MySqlConnection(ConnectionString);
            baglanti.Open();
            DuzSifreleriOzetle(baglanti);

            // Önce rol sözlüğünü doldur; kullanıcı kaydı RolAdi metnine bağlıdır.
            RoluEkle(baglanti, RolAdmin);
            RoluEkle(baglanti, RolYonetici);
            RoluEkle(baglanti, RolMuhasebeci);
            RoluEkle(baglanti, RolTeknisyen);
            RoluEkle(baglanti, RolGuvenlik);
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
                ekle.Parameters.AddWithValue("@sifre", SifreKoruma.Ozetle(AdminSifre));
                ekle.Parameters.AddWithValue("@rolAdi", RolAdmin);
                ekle.ExecuteNonQuery();
            }

            GuvenlikHesabiniHazirla(baglanti);
        }

        /// <summary>
        /// Demo güvenlik hesabı yoksa bir kez oluşturulur. Var olan şifre korunur.
        /// </summary>
        private static void GuvenlikHesabiniHazirla(MySqlConnection baglanti)
        {
            using MySqlCommand kontrol = new MySqlCommand(
                "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi = @kullaniciAdi", baglanti);
            kontrol.Parameters.AddWithValue("@kullaniciAdi", GuvenlikKullaniciAdi);
            int kayitSayisi = Convert.ToInt32(kontrol.ExecuteScalar());

            if (kayitSayisi == 0)
            {
                using MySqlCommand ekle = new MySqlCommand(
                    @"INSERT INTO Kullanicilar (Ad, Soyad, KullaniciAdi, Sifre, RolAdi)
                      VALUES (@ad, @soyad, @kullaniciAdi, @sifre, @rolAdi)", baglanti);
                ekle.Parameters.AddWithValue("@ad", "Güvenlik");
                ekle.Parameters.AddWithValue("@soyad", "Personeli");
                ekle.Parameters.AddWithValue("@kullaniciAdi", GuvenlikKullaniciAdi);
                ekle.Parameters.AddWithValue("@sifre", SifreKoruma.Ozetle(GuvenlikSifre));
                ekle.Parameters.AddWithValue("@rolAdi", RolGuvenlik);
                ekle.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Eski düz metin şifreleri bir kez özete çevirir. Özet olan satıra dokunmaz.
        /// </summary>
        private static void DuzSifreleriOzetle(MySqlConnection baglanti)
        {
            try
            {
                using MySqlCommand genislet = new MySqlCommand(
                    "ALTER TABLE Kullanicilar MODIFY Sifre VARCHAR(255) NOT NULL", baglanti);
                genislet.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                // Sütun zaten uygunsa veya yetki yoksa mevcut kayıtlarla devam edilir.
            }

            using MySqlCommand oku = new MySqlCommand(
                "SELECT KullaniciID, Sifre FROM Kullanicilar", baglanti);
            using MySqlDataReader okuyucu = oku.ExecuteReader();
            List<(int Id, string Duz)> bekleyen = new List<(int, string)>();
            while (okuyucu.Read())
            {
                string kayit = okuyucu.IsDBNull(1) ? "" : okuyucu.GetString(1);
                if (!SifreKoruma.OzetMi(kayit) && kayit.Length > 0)
                {
                    bekleyen.Add((okuyucu.GetInt32(0), kayit));
                }
            }

            okuyucu.Close();
            foreach ((int id, string duz) in bekleyen)
            {
                using MySqlCommand yaz = new MySqlCommand(
                    "UPDATE Kullanicilar SET Sifre = @sifre WHERE KullaniciID = @id", baglanti);
                yaz.Parameters.AddWithValue("@sifre", SifreKoruma.Ozetle(duz));
                yaz.Parameters.AddWithValue("@id", id);
                yaz.ExecuteNonQuery();
            }
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
