using System.Data;
using System.Globalization;
using System.Net.Sockets;
using MySqlConnector;

namespace ApartmanMobil;

public static class BaglantiAyar
{
    public const string OnerilenAgAdresi = "192.168.1.2";
    private const string VarsayilanKullanici = "apartman_panel";
    private const string VarsayilanSifre = "";

    public static string Sunucu => Preferences.Get("db_server", OnerilenAgAdresi);
    public static string Port => Preferences.Get("db_port", "3306");
    public static string VeritabaniAdi => Preferences.Get("db_name", "ApartmanYonetimDB");
    public static string Kullanici => Preferences.Get("db_user_v2", VarsayilanKullanici);
    public static string Sifre => Preferences.Get("db_pwd_v2", VarsayilanSifre);

    public static string ConnectionString
    {
        get
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = Sunucu,
                Port = uint.TryParse(Port, out uint port) ? port : 3306,
                Database = VeritabaniAdi,
                UserID = Kullanici,
                Password = Sifre,
                SslMode = MySqlSslMode.None,
                AllowPublicKeyRetrieval = true,
                ConnectionTimeout = 8
            };
            return builder.ConnectionString;
        }
    }

    public static void Kaydet(string sunucu, string port, string veritabani, string kullanici, string sifre)
    {
        Preferences.Set("db_server", sunucu.Trim());
        Preferences.Set("db_port", string.IsNullOrWhiteSpace(port) ? "3306" : port.Trim());
        Preferences.Set("db_name", veritabani.Trim());
        Preferences.Set("db_user_v2", kullanici.Trim());
        Preferences.Set("db_pwd_v2", sifre);
    }

    /// <summary>
    /// Kayıtlı adresi, Wi-Fi adresini ve USB yönlendirmesini dener.
    /// İlk açılanı kaydeder. Dosya aktarımı tek başına veritabanı yolu açmaz.
    /// </summary>
    public static string UlasilabilirAdresiSec()
    {
        string[] adaylar = { Sunucu, OnerilenAgAdresi, "127.0.0.1" };
        foreach (string aday in adaylar.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (!PortAcik(aday))
            {
                continue;
            }

            if (!string.Equals(aday, Sunucu, StringComparison.OrdinalIgnoreCase))
            {
                Preferences.Set("db_server", aday);
            }

            return aday;
        }

        return Sunucu;
    }

    private static bool PortAcik(string sunucu)
    {
        try
        {
            using TcpClient istemci = new TcpClient();
            Task baglanti = istemci.ConnectAsync(sunucu, PortNo());
            if (!baglanti.Wait(TimeSpan.FromSeconds(3)))
            {
                return false;
            }

            return istemci.Connected;
        }
        catch
        {
            return false;
        }
    }

    private static int PortNo()
    {
        return int.TryParse(Port, out int port) && port > 0 ? port : 3306;
    }
}

public static class Veritabani
{
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
    public const string AdminKullaniciAdi = "admin";
    public const string AdminSifre = "1234";

    public static MySqlConnection BaglantiAc()
    {
        MySqlConnection baglanti = new MySqlConnection(BaglantiAyar.ConnectionString);
        baglanti.Open();
        return baglanti;
    }

    public static List<Satir> Satirlar(string sql, params (string Ad, object? Deger)[] parametreler)
    {
        using MySqlConnection baglanti = BaglantiAc();
        using MySqlCommand komut = Komut(baglanti, sql, parametreler);
        using MySqlDataAdapter adaptor = new MySqlDataAdapter(komut);
        DataTable tablo = new DataTable();
        adaptor.Fill(tablo);

        List<Satir> liste = new List<Satir>();
        foreach (DataRow row in tablo.Rows)
        {
            Satir satir = new Satir();
            foreach (DataColumn kolon in tablo.Columns)
            {
                satir.Ekle(kolon.ColumnName, row[kolon]);
            }

            liste.Add(satir);
        }

        return liste;
    }

    public static object? Skaler(string sql, params (string Ad, object? Deger)[] parametreler)
    {
        using MySqlConnection baglanti = BaglantiAc();
        using MySqlCommand komut = Komut(baglanti, sql, parametreler);
        return komut.ExecuteScalar();
    }

    public static int Sayi(string sql, params (string Ad, object? Deger)[] parametreler)
    {
        return Convert.ToInt32(Skaler(sql, parametreler) ?? 0);
    }

    public static int Calistir(string sql, params (string Ad, object? Deger)[] parametreler)
    {
        using MySqlConnection baglanti = BaglantiAc();
        using MySqlCommand komut = Komut(baglanti, sql, parametreler);
        return komut.ExecuteNonQuery();
    }

    public static List<ListeKalemi> DaireListesi()
    {
        List<Satir> tablo = Satirlar(
            @"SELECT DaireID, CONCAT(IFNULL(Blok, '-'), ' / ', DaireNo) AS Etiket
              FROM Daireler
              ORDER BY Blok, DaireNo");
        return tablo.Select(s => new ListeKalemi(s.Tam("DaireID"), s.Metin("Etiket"))).ToList();
    }

    public static List<ListeKalemi> KullaniciDaireleri(int kullaniciId)
    {
        List<Satir> tablo = Satirlar(
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
        return tablo.Select(s => new ListeKalemi(s.Tam("DaireID"), s.Metin("Etiket"))).ToList();
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

    public static void AdminHesabiniHazirla()
    {
        using MySqlConnection baglanti = BaglantiAc();
        DuzSifreleriOzetle(baglanti);
        RoluEkle(baglanti, RolAdmin);
        RoluEkle(baglanti, RolYonetici);
        RoluEkle(baglanti, RolMuhasebeci);
        RoluEkle(baglanti, RolTeknisyen);
        RoluEkle(baglanti, RolGuvenlik);
        RoluEkle(baglanti, RolSakin);

        using MySqlCommand kontrol = new MySqlCommand(
            "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi = @kullaniciAdi", baglanti);
        kontrol.Parameters.AddWithValue("@kullaniciAdi", AdminKullaniciAdi);
        int kayitSayisi = Convert.ToInt32(kontrol.ExecuteScalar());

        if (kayitSayisi == 0)
        {
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

    public static string BaglantiIpucu(Exception ex)
    {
        string metin = ex.ToString();
        if (metin.Contains("Unable to connect", StringComparison.OrdinalIgnoreCase)
            || metin.Contains("timeout", StringComparison.OrdinalIgnoreCase)
            || metin.Contains("actively refused", StringComparison.OrdinalIgnoreCase)
            || ex.InnerException is System.Net.Sockets.SocketException)
        {
            return "\n\nTelefon bilgisayara ulaşamadı. Telefon ve bilgisayar aynı Wi-Fi ağında olmalı; sunucu adresi "
                + BaglantiAyar.OnerilenAgAdresi
                + ". Dosya aktarımı veritabanını açmaz. 127.0.0.1 yalnızca USB hata ayıklama izni verildiyse çalışır.";
        }

        return "";
    }

    private static void RoluEkle(MySqlConnection baglanti, string rolAdi)
    {
        try
        {
            using MySqlCommand kontrol = new MySqlCommand(
                "SELECT COUNT(*) FROM Roller WHERE RolAdi = @rolAdi", baglanti);
            kontrol.Parameters.AddWithValue("@rolAdi", rolAdi);
            if (Convert.ToInt32(kontrol.ExecuteScalar()) > 0)
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
        }
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
}

public sealed class ListeKalemi
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

public sealed class Satir
{
    private readonly Dictionary<string, object?> _ham = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

    public void Ekle(string ad, object? deger) => _ham[ad] = deger is DBNull ? null : deger;

    public string Metin(string ad)
    {
        if (!_ham.TryGetValue(ad, out object? deger) || deger is null)
        {
            return "";
        }

        CultureInfo tr = new CultureInfo("tr-TR");
        return deger switch
        {
            DateTime tarih when tarih.TimeOfDay == TimeSpan.Zero => tarih.ToString("dd.MM.yyyy", tr),
            DateTime tarih => tarih.ToString("dd.MM.yyyy HH:mm", tr),
            decimal or double or float => Convert.ToDecimal(deger, CultureInfo.InvariantCulture).ToString("N2", tr),
            _ => Convert.ToString(deger, tr) ?? ""
        };
    }

    public decimal Sayi(string ad)
    {
        if (!_ham.TryGetValue(ad, out object? deger) || deger is null)
        {
            return 0;
        }

        return Convert.ToDecimal(deger, CultureInfo.InvariantCulture);
    }

    public int Tam(string ad)
    {
        if (!_ham.TryGetValue(ad, out object? deger) || deger is null)
        {
            return 0;
        }

        return Convert.ToInt32(deger, CultureInfo.InvariantCulture);
    }
}

public sealed record Oturum(int Id, string AdSoyad, string Rol)
{
    public bool AdminMi => Esit(Veritabani.RolAdmin);
    public bool YoneticiMi => Esit(Veritabani.RolYonetici);
    public bool MuhasebeciMi => Esit(Veritabani.RolMuhasebeci);
    public bool TeknisyenMi => Esit(Veritabani.RolTeknisyen);
    public bool GuvenlikMi => Esit(Veritabani.RolGuvenlik);
    public bool SakinMi => Esit(Veritabani.RolSakin);

    private bool Esit(string rol) => string.Equals(Rol, rol, StringComparison.OrdinalIgnoreCase);
}

