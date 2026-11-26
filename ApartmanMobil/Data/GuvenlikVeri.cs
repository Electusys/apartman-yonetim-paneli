using MySqlConnector;

namespace ApartmanMobil;

public static class GuvenlikVeri
{
    public static void Yaz(int kullaniciId, string tur, string aciklama)
    {
        if (aciklama.Length > 500)
        {
            aciklama = aciklama[..500];
        }

        Veritabani.Calistir(
            @"INSERT INTO GuvenlikIslemKayitlari (KullaniciID, IslemTuru, Aciklama)
              VALUES (@id, @tur, @aciklama)",
            ("@id", kullaniciId),
            ("@tur", tur),
            ("@aciklama", aciklama));
    }

    public static string Plaka(string ham)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (char c in ham.Trim().ToUpperInvariant())
        {
            if (char.IsLetterOrDigit(c))
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    public static bool PlakaGecerli(string plaka) => plaka.Length is >= 5 and <= 15;

    public static List<Satir> Ziyaretler()
    {
        return Veritabani.Satirlar(
            @"SELECT z.ZiyaretID, z.Tur, z.AdSoyad,
                     CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                     IFNULL(z.Plaka, '') AS Plaka,
                     DATE_FORMAT(z.GirisZamani, '%d.%m.%Y %H:%i') AS Giris,
                     IFNULL(DATE_FORMAT(z.CikisZamani, '%d.%m.%Y %H:%i'), '') AS Cikis
              FROM ZiyaretKayitlari z
              INNER JOIN Daireler d ON d.DaireID = z.DaireID
              ORDER BY z.ZiyaretID DESC");
    }

    public static Sonuc ZiyaretEkle(int kullaniciId, string tur, string adSoyad, int daireId, string? plaka)
    {
        if (string.IsNullOrWhiteSpace(adSoyad))
        {
            return new Sonuc(false, "Ad soyad zorunludur.");
        }

        if (daireId <= 0)
        {
            return new Sonuc(false, "Ziyaret edilen daireyi seçin.");
        }

        Veritabani.Calistir(
            @"INSERT INTO ZiyaretKayitlari (Tur, AdSoyad, DaireID, GirisZamani, Plaka, KaydedenKullaniciID)
              VALUES (@tur, @ad, @daire, NOW(), @plaka, @kullanici)",
            ("@tur", tur),
            ("@ad", adSoyad.Trim()),
            ("@daire", daireId),
            ("@plaka", plaka),
            ("@kullanici", kullaniciId));
        Yaz(kullaniciId, "Ziyaret girişi", tur + " girişi kaydedildi. " + adSoyad.Trim());
        return new Sonuc(true, "Giriş kaydı oluşturuldu.");
    }

    public static Sonuc ZiyaretCikis(int kullaniciId, int ziyaretId)
    {
        int etkilenen = Veritabani.Calistir(
            @"UPDATE ZiyaretKayitlari SET CikisZamani = NOW()
              WHERE ZiyaretID = @id AND CikisZamani IS NULL",
            ("@id", ziyaretId));
        if (etkilenen == 0)
        {
            return new Sonuc(false, "Bu kaydın çıkış saati zaten yazılmış.");
        }

        Yaz(kullaniciId, "Ziyaret çıkışı", "Ziyaret " + ziyaretId + " için çıkış saati yazıldı.");
        return new Sonuc(true, "Çıkış saati kaydedildi.");
    }

    public static List<Satir> Kargolar()
    {
        return Veritabani.Satirlar(
            @"SELECT k.KargoID, CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                     k.KargoBilgisi, DATE_FORMAT(k.TeslimAlmaZamani, '%d.%m.%Y %H:%i') AS Teslim
              FROM Kargolar k
              INNER JOIN Daireler d ON d.DaireID = k.DaireID
              ORDER BY k.KargoID DESC");
    }

    public static Sonuc KargoEkle(int kullaniciId, int daireId, string bilgi)
    {
        if (string.IsNullOrWhiteSpace(bilgi))
        {
            return new Sonuc(false, "Kargo bilgisi zorunludur.");
        }

        if (daireId <= 0)
        {
            return new Sonuc(false, "Kargonun bırakılacağı daireyi seçin.");
        }

        using MySqlConnection baglanti = Veritabani.BaglantiAc();
        using MySqlTransaction islem = baglanti.BeginTransaction();
        using (MySqlCommand ekle = new MySqlCommand(
            @"INSERT INTO Kargolar (DaireID, KargoBilgisi, TeslimAlmaZamani, KaydedenKullaniciID)
              VALUES (@daire, @bilgi, NOW(), @kullanici)", baglanti, islem))
        {
            ekle.Parameters.AddWithValue("@daire", daireId);
            ekle.Parameters.AddWithValue("@bilgi", bilgi.Trim());
            ekle.Parameters.AddWithValue("@kullanici", kullaniciId);
            ekle.ExecuteNonQuery();
            long kargoId = ekle.LastInsertedId;
            using MySqlCommand bildirim = new MySqlCommand(
                @"INSERT INTO DaireBildirimleri (DaireID, Baslik, Icerik, KargoID, OlusturanKullaniciID)
                  VALUES (@daire, @baslik, @icerik, @kargo, @kullanici)", baglanti, islem);
            bildirim.Parameters.AddWithValue("@daire", daireId);
            bildirim.Parameters.AddWithValue("@baslik", "Kargo teslim alındı");
            bildirim.Parameters.AddWithValue("@icerik", "Güvenlik bir kargo teslim aldı: " + bilgi.Trim());
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
            gunluk.Parameters.AddWithValue("@aciklama", "Kargo kaydı ve daire bildirimi oluşturuldu. " + bilgi.Trim());
            gunluk.ExecuteNonQuery();
        }

        islem.Commit();
        return new Sonuc(true, "Kargo kaydedildi ve daire bildirimi oluşturuldu.");
    }

    public static List<Satir> Rehber(string aranan)
    {
        return Veritabani.Satirlar(
            @"SELECT Ad, Soyad, Daire, IFNULL(AcilIletisim, '') AS AcilIletisim
              FROM vw_GuvenlikSakinRehberi
              WHERE @ara = '' OR Ad LIKE @like OR Soyad LIKE @like OR Daire LIKE @like OR AcilIletisim LIKE @like
              ORDER BY Daire, Ad, Soyad",
            ("@ara", aranan),
            ("@like", "%" + aranan + "%"));
    }

    public static string PlakaSorgula(int kullaniciId, string plaka)
    {
        List<Satir> kayit = Veritabani.Satirlar(
            "SELECT Daire, IFNULL(Sakinler, '') AS Sakinler FROM vw_GuvenlikPlaka WHERE Plaka = @plaka",
            ("@plaka", plaka));
        List<Satir> izin = Veritabani.Satirlar(
            @"SELECT CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                     DATE_FORMAT(p.BaslangicZamani, '%d.%m.%Y %H:%i') AS Baslangic,
                     DATE_FORMAT(p.BitisZamani, '%d.%m.%Y %H:%i') AS Bitis
              FROM GeciciParkIzinleri p
              INNER JOIN Daireler d ON d.DaireID = p.DaireID
              WHERE p.Plaka = @plaka AND p.BaslangicZamani <= NOW() AND p.BitisZamani >= NOW()
              ORDER BY p.BitisZamani DESC LIMIT 1",
            ("@plaka", plaka));
        string sonuc = kayit.Count == 0
            ? plaka + " kayıtlı araçlarda yok."
            : plaka + " kayıtlı. Daire: " + kayit[0].Metin("Daire") + "."
              + (string.IsNullOrWhiteSpace(kayit[0].Metin("Sakinler")) ? "" : " Sakin: " + kayit[0].Metin("Sakinler") + ".");
        if (izin.Count > 0)
        {
            sonuc += " Aktif geçici izin: " + izin[0].Metin("Daire") + ", " + izin[0].Metin("Baslangic") + " - " + izin[0].Metin("Bitis") + ".";
        }

        Yaz(kullaniciId, "Plaka sorgusu", sonuc);
        return sonuc;
    }

    public static List<Satir> ParkIzinleri()
    {
        return Veritabani.Satirlar(
            @"SELECT p.Plaka, p.ZiyaretciAdSoyad,
                     CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                     DATE_FORMAT(p.BaslangicZamani, '%d.%m.%Y %H:%i') AS Baslangic,
                     DATE_FORMAT(p.BitisZamani, '%d.%m.%Y %H:%i') AS Bitis
              FROM GeciciParkIzinleri p
              INNER JOIN Daireler d ON d.DaireID = p.DaireID
              ORDER BY p.IzinID DESC");
    }

    public static Sonuc ParkIzni(int kullaniciId, string plaka, string ad, int daireId, DateTime baslangic, DateTime bitis)
    {
        if (!PlakaGecerli(plaka) || string.IsNullOrWhiteSpace(ad))
        {
            return new Sonuc(false, "Plaka ve ziyaretçi ad soyad zorunludur.");
        }

        if (daireId <= 0)
        {
            return new Sonuc(false, "İznin bağlı olduğu daireyi seçin.");
        }

        if (bitis <= baslangic)
        {
            return new Sonuc(false, "Bitiş zamanı başlangıçtan sonra olmalıdır.");
        }

        Veritabani.Calistir(
            @"INSERT INTO GeciciParkIzinleri
                (Plaka, DaireID, ZiyaretciAdSoyad, BaslangicZamani, BitisZamani, KaydedenKullaniciID)
              VALUES (@plaka, @daire, @ad, @bas, @bit, @kullanici)",
            ("@plaka", plaka),
            ("@daire", daireId),
            ("@ad", ad.Trim()),
            ("@bas", baslangic),
            ("@bit", bitis),
            ("@kullanici", kullaniciId));
        Yaz(kullaniciId, "Park izni", plaka + " için geçici izin yazıldı.");
        return new Sonuc(true, "Geçici park izni oluşturuldu.");
    }

    public static List<Satir> Olaylar()
    {
        return Veritabani.Satirlar(
            @"SELECT o.Tur, o.Aciklama, DATE_FORMAT(o.OlusturmaTarihi, '%d.%m.%Y %H:%i') AS Zaman,
                     CONCAT(k.Ad, ' ', k.Soyad) AS Personel
              FROM OlayKayitlari o
              INNER JOIN Kullanicilar k ON k.KullaniciID = o.OlusturanKullaniciID
              ORDER BY o.OlayID DESC");
    }

    public static Sonuc OlayEkle(int kullaniciId, string tur, string aciklama)
    {
        if (string.IsNullOrWhiteSpace(aciklama))
        {
            return new Sonuc(false, "Tür ve açıklama zorunludur.");
        }

        Veritabani.Calistir(
            @"INSERT INTO OlayKayitlari (Tur, Aciklama, OlusturanKullaniciID)
              VALUES (@tur, @aciklama, @kullanici)",
            ("@tur", tur),
            ("@aciklama", aciklama.Trim()),
            ("@kullanici", kullaniciId));
        Yaz(kullaniciId, "Olay raporu", tur + " kaydı oluşturuldu.");
        return new Sonuc(true, "Olay kaydı oluşturuldu. Bu kayıt değiştirilemez.");
    }

    public static List<Satir> Aciller()
    {
        return Veritabani.Satirlar(
            @"SELECT a.Tur, IFNULL(a.NotMetni, '') AS NotMetni,
                     DATE_FORMAT(a.OlusturmaTarihi, '%d.%m.%Y %H:%i') AS Zaman
              FROM AcilDurumKayitlari a
              ORDER BY a.AcilID DESC");
    }

    public static Sonuc AcilEkle(int kullaniciId, string tur, string? not)
    {
        Veritabani.Calistir(
            @"INSERT INTO AcilDurumKayitlari (Tur, NotMetni, OlusturanKullaniciID)
              VALUES (@tur, @not, @kullanici)",
            ("@tur", tur),
            ("@not", string.IsNullOrWhiteSpace(not) ? null : not.Trim()),
            ("@kullanici", kullaniciId));
        Yaz(kullaniciId, "Acil durum", tur + " kağıt kaydı oluşturuldu. Dış bildirim gönderilmedi.");
        return new Sonuc(true, tur + " kaydı yazıldı. Arama yapılmadı.");
    }

    public static List<Satir> Biletlerim(int kullaniciId)
    {
        return Veritabani.Satirlar(
            @"SELECT b.Konu, IFNULL(b.SakinAdSoyad, '') AS Sakin,
                     IFNULL(CONCAT(d.Blok, ' / ', d.DaireNo), '') AS Daire,
                     DATE_FORMAT(b.OlusturmaTarihi, '%d.%m.%Y %H:%i') AS Zaman
              FROM YonetimBiletleri b
              LEFT JOIN Daireler d ON d.DaireID = b.DaireID
              WHERE b.OlusturanKullaniciID = @id
              ORDER BY b.BiletID DESC",
            ("@id", kullaniciId));
    }

    public static List<Satir> TumBiletler()
    {
        return Veritabani.Satirlar(
            @"SELECT b.Konu, b.Aciklama, IFNULL(b.SakinAdSoyad, '') AS Sakin,
                     IFNULL(CONCAT(d.Blok, ' / ', d.DaireNo), '') AS Daire,
                     CONCAT(k.Ad, ' ', k.Soyad) AS Olusturan,
                     DATE_FORMAT(b.OlusturmaTarihi, '%d.%m.%Y %H:%i') AS Zaman
              FROM YonetimBiletleri b
              INNER JOIN Kullanicilar k ON k.KullaniciID = b.OlusturanKullaniciID
              LEFT JOIN Daireler d ON d.DaireID = b.DaireID
              ORDER BY b.BiletID DESC");
    }

    public static Sonuc BiletEkle(int kullaniciId, string konu, string aciklama, string? sakin, int? daireId)
    {
        if (string.IsNullOrWhiteSpace(konu) || string.IsNullOrWhiteSpace(aciklama))
        {
            return new Sonuc(false, "Konu ve açıklama zorunludur.");
        }

        Veritabani.Calistir(
            @"INSERT INTO YonetimBiletleri (Konu, Aciklama, SakinAdSoyad, DaireID, OlusturanKullaniciID)
              VALUES (@konu, @aciklama, @sakin, @daire, @kullanici)",
            ("@konu", konu.Trim()),
            ("@aciklama", aciklama.Trim()),
            ("@sakin", string.IsNullOrWhiteSpace(sakin) ? null : sakin.Trim()),
            ("@daire", daireId),
            ("@kullanici", kullaniciId));
        Yaz(kullaniciId, "Yönetim bileti", "Yöneticiye bilet iletildi: " + konu.Trim());
        return new Sonuc(true, "Bilet yöneticiye iletildi.");
    }

    public static List<Satir> Araclar()
    {
        return Veritabani.Satirlar(
            @"SELECT a.AracID, a.Plaka, CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire
              FROM KayitliAraclar a
              INNER JOIN Daireler d ON d.DaireID = a.DaireID
              ORDER BY a.Plaka");
    }

    public static Sonuc AracEkle(int kullaniciId, string plaka, int daireId)
    {
        if (!PlakaGecerli(plaka))
        {
            return new Sonuc(false, "Geçerli bir plaka girin.");
        }

        if (daireId <= 0)
        {
            return new Sonuc(false, "Aracın bağlı olduğu daireyi seçin.");
        }

        try
        {
            Veritabani.Calistir(
                @"INSERT INTO KayitliAraclar (DaireID, Plaka, KaydedenKullaniciID)
                  VALUES (@daire, @plaka, @kullanici)",
                ("@daire", daireId),
                ("@plaka", plaka),
                ("@kullanici", kullaniciId));
            return new Sonuc(true, "Araç kaydı oluşturuldu.");
        }
        catch (MySqlException ex) when (ex.Number == 1062)
        {
            return new Sonuc(false, "Bu plaka zaten kayıtlı.");
        }
    }

    public static void AracSil(int aracId)
    {
        Veritabani.Calistir("DELETE FROM KayitliAraclar WHERE AracID = @id", ("@id", aracId));
    }

    public static List<Satir> Gunluk()
    {
        return Veritabani.Satirlar(
            @"SELECT DATE_FORMAT(i.IslemTarihi, '%d.%m.%Y %H:%i') AS Zaman,
                     CONCAT(k.Ad, ' ', k.Soyad) AS Personel, i.IslemTuru, i.Aciklama
              FROM GuvenlikIslemKayitlari i
              INNER JOIN Kullanicilar k ON k.KullaniciID = i.KullaniciID
              ORDER BY i.IslemID DESC
              LIMIT 80");
    }

    public static PanelVerisi Panel(int kullaniciId)
    {
        return new PanelVerisi
        {
            Blok = Veritabani.Sayi("SELECT COUNT(*) FROM ZiyaretKayitlari WHERE DATE(GirisZamani) = CURDATE()").ToString(),
            Daire = Veritabani.Sayi("SELECT COUNT(*) FROM Kargolar").ToString(),
            Sakin = Veritabani.Sayi("SELECT COUNT(*) FROM GeciciParkIzinleri WHERE BaslangicZamani <= NOW() AND BitisZamani >= NOW()").ToString(),
            Ariza = Veritabani.Sayi("SELECT COUNT(*) FROM YonetimBiletleri WHERE OlusturanKullaniciID = @id", ("@id", kullaniciId)).ToString(),
            Duyurular = Veritabani.Satirlar(
                @"SELECT IslemTuru AS Gorev, DATE_FORMAT(IslemTarihi, '%d.%m.%Y %H:%i') AS Tarih
                  FROM GuvenlikIslemKayitlari
                  WHERE KullaniciID = @id
                  ORDER BY IslemID DESC
                  LIMIT 8",
                ("@id", kullaniciId))
        };
    }
}
