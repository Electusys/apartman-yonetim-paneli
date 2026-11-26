namespace ApartmanMobil;

public sealed class PanelVerisi
{
    public string Blok { get; init; } = "—";
    public string Daire { get; init; } = "—";
    public string Sakin { get; init; } = "—";
    public string Ariza { get; init; } = "—";
    public List<Satir> Duyurular { get; init; } = new List<Satir>();
}

public static class Listeler
{
    public static PanelVerisi Panel()
    {
        return new PanelVerisi
        {
            Blok = Veritabani.Sayi("SELECT COUNT(*) FROM Bloklar").ToString(),
            Daire = Veritabani.Sayi("SELECT COUNT(*) FROM Daireler").ToString(),
            Sakin = Veritabani.Sayi("SELECT COUNT(*) FROM Sakinler WHERE Aktif = 1").ToString(),
            Ariza = Veritabani.Sayi(
                "SELECT COUNT(*) FROM ArizaBildirimleri WHERE Durum <> @d",
                ("@d", Veritabani.ArizaTamamlandi)).ToString(),
            Duyurular = Veritabani.Satirlar(
                @"SELECT Baslik AS Gorev, DATE_FORMAT(YayimTarihi, '%d.%m.%Y') AS Tarih
                  FROM Duyurular
                  ORDER BY YayimTarihi DESC
                  LIMIT 8")
        };
    }

    public static List<Satir> Yoneticiler()
    {
        return Veritabani.Satirlar(
            @"SELECT KullaniciID, Ad, Soyad, KullaniciAdi, RolAdi
              FROM Kullanicilar
              WHERE RolAdi = @rol
              ORDER BY KullaniciID DESC",
            ("@rol", Veritabani.RolYonetici));
    }

    public static List<Satir> Personeller()
    {
        return Veritabani.Satirlar(
            @"SELECT KullaniciID, Ad, Soyad, KullaniciAdi, RolAdi
              FROM Kullanicilar
              WHERE RolAdi IN (@muhasebeci, @teknisyen, @guvenlik)
              ORDER BY RolAdi, KullaniciID DESC",
            ("@muhasebeci", Veritabani.RolMuhasebeci),
            ("@teknisyen", Veritabani.RolTeknisyen),
            ("@guvenlik", Veritabani.RolGuvenlik));
    }

    public static List<Satir> Bloklar()
    {
        return Veritabani.Satirlar(
            "SELECT BlokID, BlokAdi, KatSayisi, Aciklama FROM Bloklar ORDER BY BlokID DESC");
    }

    public static List<Satir> Daireler()
    {
        return Veritabani.Satirlar(
            "SELECT DaireID, Blok, DaireNo, Kat, KullaniciID FROM Daireler ORDER BY Blok, DaireNo");
    }

    public static List<string> BlokAdlari()
    {
        return Veritabani.Satirlar("SELECT BlokAdi FROM Bloklar ORDER BY BlokAdi")
            .Select(s => s.Metin("BlokAdi"))
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();
    }

    public static List<Satir> Sakinler()
    {
        return Veritabani.Satirlar(
            @"SELECT s.SakinID, s.Ad, s.Soyad, s.Telefon,
                     CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                     IFNULL(k.KullaniciAdi, '') AS KullaniciAdi,
                     CASE WHEN s.Aktif = 1 THEN 'Aktif' ELSE 'Pasif' END AS Durum
              FROM Sakinler s
              INNER JOIN Daireler d ON d.DaireID = s.DaireID
              LEFT JOIN Kullanicilar k ON k.KullaniciID = s.KullaniciID
              ORDER BY s.SakinID DESC");
    }

    public static List<ListeKalemi> SakinHesaplari()
    {
        List<ListeKalemi> liste = new List<ListeKalemi> { new ListeKalemi(0, "(Bağlama)") };
        liste.AddRange(Veritabani.Satirlar(
            @"SELECT KullaniciID, CONCAT(Ad, ' ', Soyad, ' (', KullaniciAdi, ')') AS Etiket
              FROM Kullanicilar
              WHERE RolAdi = @rol
              ORDER BY Ad, Soyad",
            ("@rol", Veritabani.RolSakin))
            .Select(s => new ListeKalemi(s.Tam("KullaniciID"), s.Metin("Etiket"))));
        return liste;
    }

    public static List<Satir> AidatBorclari()
    {
        return Veritabani.Satirlar(
            @"SELECT b.AidatBorcID, dnm.Donem, CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                     b.Tutar, b.KalanTutar, b.Durum, dnm.SonOdemeTarihi
              FROM AidatBorclari b
              INNER JOIN AidatDonemleri dnm ON dnm.AidatDonemID = b.AidatDonemID
              INNER JOIN Daireler d ON d.DaireID = b.DaireID
              ORDER BY dnm.Donem DESC, d.Blok, d.DaireNo");
    }

    public static List<Satir> AcikBorclar()
    {
        return Veritabani.Satirlar(
            @"SELECT b.AidatBorcID, CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire, dnm.Donem,
                     b.Tutar, b.KalanTutar, b.Durum
              FROM AidatBorclari b
              INNER JOIN Daireler d ON d.DaireID = b.DaireID
              INNER JOIN AidatDonemleri dnm ON dnm.AidatDonemID = b.AidatDonemID
              ORDER BY b.KalanTutar DESC, dnm.Donem DESC");
    }

    public static List<Satir> Odemeler()
    {
        return Veritabani.Satirlar(
            @"SELECT o.OdemeID, CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire, dnm.Donem,
                     o.Tutar, o.OdemeTarihi, o.OdemeYontemi
              FROM Odemeler o
              INNER JOIN AidatBorclari b ON b.AidatBorcID = o.AidatBorcID
              INNER JOIN Daireler d ON d.DaireID = b.DaireID
              INNER JOIN AidatDonemleri dnm ON dnm.AidatDonemID = b.AidatDonemID
              ORDER BY o.OdemeID DESC");
    }

    public static List<Satir> Giderler()
    {
        return Veritabani.Satirlar(
            @"SELECT GiderID, Kategori, Aciklama, Tutar, Tarih
              FROM Giderler
              ORDER BY GiderID DESC");
    }

    public static List<Satir> Duyurular(bool yonetici)
    {
        return Veritabani.Satirlar(
            @"SELECT d.DuyuruID, d.Baslik, d.Icerik,
                     CONCAT(IFNULL(k.Ad, ''), ' ', IFNULL(k.Soyad, '')) AS Yazan,
                     d.YayimTarihi,
                     (SELECT COUNT(*) FROM DuyuruYorumlari y WHERE y.DuyuruID = d.DuyuruID) AS YorumSayisi
              FROM Duyurular d
              LEFT JOIN Kullanicilar k ON k.KullaniciID = d.OlusturanKullaniciID
              WHERE d.YayimTarihi <= NOW() OR @yonetici = 1
              ORDER BY d.YayimTarihi DESC",
            ("@yonetici", yonetici ? 1 : 0));
    }

    public static List<Satir> Yorumlar(int duyuruId)
    {
        return Veritabani.Satirlar(
            @"SELECT CONCAT(k.Ad, ' ', k.Soyad) AS Yazan, k.RolAdi AS Rol, y.Yorum, y.Tarih
              FROM DuyuruYorumlari y
              INNER JOIN Kullanicilar k ON k.KullaniciID = y.KullaniciID
              WHERE y.DuyuruID = @id
              ORDER BY y.Tarih",
            ("@id", duyuruId));
    }

    public static List<Satir> Arizalar()
    {
        return Veritabani.Satirlar(
            @"SELECT a.ArizaID, CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                     CONCAT(k.Ad, ' ', k.Soyad) AS Bildiren, a.Alan, a.Kategori, a.Aciklama,
                     a.Oncelik, a.Durum, a.CozumNotu, a.OlusturmaTarihi
              FROM ArizaBildirimleri a
              INNER JOIN Daireler d ON d.DaireID = a.DaireID
              INNER JOIN Kullanicilar k ON k.KullaniciID = a.BildirenKullaniciID
              ORDER BY a.ArizaID DESC");
    }

    public static List<Satir> Aidatlarim(int kullaniciId)
    {
        return Veritabani.Satirlar(
            @"SELECT CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire, dnm.Donem,
                     b.Tutar, b.KalanTutar, b.Durum, dnm.SonOdemeTarihi
              FROM AidatBorclari b
              INNER JOIN Daireler d ON d.DaireID = b.DaireID
              INNER JOIN AidatDonemleri dnm ON dnm.AidatDonemID = b.AidatDonemID
              WHERE d.DaireID IN (
                  SELECT DaireID FROM Sakinler WHERE KullaniciID = @id AND Aktif = 1
                  UNION
                  SELECT DaireID FROM Daireler WHERE KullaniciID = @id
              )
              ORDER BY dnm.Donem DESC",
            ("@id", kullaniciId));
    }
}
