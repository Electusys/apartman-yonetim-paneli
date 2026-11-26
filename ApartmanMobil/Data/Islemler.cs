using MySqlConnector;

namespace ApartmanMobil;

public readonly record struct Sonuc(bool Tamam, string Mesaj);

public readonly record struct GirisSonucu(bool Basarili, string Mesaj, Oturum? Oturum);

public static class Islemler
{
    public static GirisSonucu Giris(string kullaniciAdi, string sifre)
    {
        if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
        {
            return new GirisSonucu(false, "Kullanıcı adı ve şifre boş bırakılamaz.", null);
        }

        List<Satir> tablo = Veritabani.Satirlar(
            @"SELECT KullaniciID, Ad, Soyad, RolAdi, Durum, Sifre
              FROM Kullanicilar
              WHERE KullaniciAdi = @kullaniciAdi
              LIMIT 1",
            ("@kullaniciAdi", kullaniciAdi.Trim()));

        if (tablo.Count == 0 || !SifreKoruma.Dogrula(sifre, tablo[0].Metin("Sifre")))
        {
            return new GirisSonucu(false, "Kullanıcı adı veya şifre hatalı.", null);
        }

        Satir satir = tablo[0];
        string durum = string.IsNullOrWhiteSpace(satir.Metin("Durum")) ? Veritabani.DurumAktif : satir.Metin("Durum");
        if (string.Equals(durum, Veritabani.DurumPasif, StringComparison.OrdinalIgnoreCase))
        {
            return new GirisSonucu(false, "Kullanıcı hesabı pasif. Sisteme girişe izin verilmez.", null);
        }

        Oturum oturum = new Oturum(
            satir.Tam("KullaniciID"),
            (satir.Metin("Ad") + " " + satir.Metin("Soyad")).Trim(),
            satir.Metin("RolAdi"));
        if (oturum.GuvenlikMi)
        {
            try
            {
                GuvenlikVeri.Yaz(oturum.Id, "Oturum açıldı", "Güvenlik oturumu açıldı.");
            }
            catch
            {
                // Günlük tablosu yoksa giriş yine de açılsın.
            }
        }

        return new GirisSonucu(true, "", oturum);
    }

    public static Sonuc SakinKaydi(string ad, string soyad, string kullaniciAdi, string sifre, string sifreTekrar)
    {
        Sonuc eksik = Zorunlu(ad, soyad, kullaniciAdi, sifre, sifreTekrar);
        if (!eksik.Tamam)
        {
            return eksik;
        }

        if (sifre != sifreTekrar)
        {
            return new Sonuc(false, "Şifreler birbiriyle eşleşmiyor.");
        }

        return KullaniciEkle(ad, soyad, kullaniciAdi, sifre, Veritabani.RolSakin,
            "Bu kullanıcı adı sistem tarafından ayrılmıştır.",
            "Sakin hesabınız oluşturuldu. Giriş ekranından oturum açabilirsiniz.");
    }

    public static Sonuc YoneticiOlustur(string ad, string soyad, string kullaniciAdi, string sifre, string sifreTekrar)
    {
        Sonuc eksik = Zorunlu(ad, soyad, kullaniciAdi, sifre, sifreTekrar);
        if (!eksik.Tamam)
        {
            return eksik;
        }

        if (sifre != sifreTekrar)
        {
            return new Sonuc(false, "Şifreler birbiriyle eşleşmiyor.");
        }

        return KullaniciEkle(ad, soyad, kullaniciAdi, sifre, Veritabani.RolYonetici,
            "Admin kullanıcı adı değiştirilemez ve başkasına verilemez.",
            "Apartman yöneticisi hesabı oluşturuldu.");
    }

    public static Sonuc PersonelOlustur(string ad, string soyad, string kullaniciAdi, string sifre, string sifreTekrar, string rolAdi)
    {
        Sonuc eksik = Zorunlu(ad, soyad, kullaniciAdi, sifre, sifreTekrar);
        if (!eksik.Tamam)
        {
            return eksik;
        }

        if (sifre != sifreTekrar)
        {
            return new Sonuc(false, "Şifreler birbiriyle eşleşmiyor.");
        }

        if (rolAdi != Veritabani.RolMuhasebeci && rolAdi != Veritabani.RolTeknisyen && rolAdi != Veritabani.RolGuvenlik)
        {
            return new Sonuc(false, "Yönetici yalnızca muhasebeci, teknisyen veya güvenlik atayabilir.");
        }

        return KullaniciEkle(ad, soyad, kullaniciAdi, sifre, rolAdi,
            "Admin kullanıcı adı başkasına verilemez.",
            rolAdi + " hesabı oluşturuldu.");
    }

    public static Sonuc BlokEkle(string blokAdi, int katSayisi, string aciklama)
    {
        if (string.IsNullOrWhiteSpace(blokAdi))
        {
            return new Sonuc(false, "Zorunlu bilgiler eksik. Blok adı giriniz.");
        }

        if (Veritabani.Sayi("SELECT COUNT(*) FROM Bloklar WHERE BlokAdi = @ad", ("@ad", blokAdi.Trim())) > 0)
        {
            return new Sonuc(false, "Bu blok adı zaten kayıtlı.");
        }

        Veritabani.Calistir(
            "INSERT INTO Bloklar (BlokAdi, KatSayisi, Aciklama) VALUES (@ad, @kat, @aciklama)",
            ("@ad", blokAdi.Trim()),
            ("@kat", katSayisi),
            ("@aciklama", string.IsNullOrWhiteSpace(aciklama) ? null : aciklama.Trim()));
        return new Sonuc(true, "Blok kaydı oluşturuldu.");
    }

    public static Sonuc DaireEkle(string? blok, string daireNo, int kat)
    {
        if (string.IsNullOrWhiteSpace(blok) || string.IsNullOrWhiteSpace(daireNo))
        {
            return new Sonuc(false, "Zorunlu bilgiler eksik. Blok ve daire numarasını giriniz.");
        }

        if (Veritabani.Sayi("SELECT COUNT(*) FROM Daireler WHERE Blok = @blok AND DaireNo = @no", ("@blok", blok), ("@no", daireNo.Trim())) > 0)
        {
            return new Sonuc(false, "Aynı blokta bu daire numarası zaten var. Mükerrer kayıt engellendi.");
        }

        Veritabani.Calistir(
            "INSERT INTO Daireler (Blok, DaireNo, Kat) VALUES (@blok, @no, @kat)",
            ("@blok", blok),
            ("@no", daireNo.Trim()),
            ("@kat", kat));
        return new Sonuc(true, "Daire kaydı oluşturuldu.");
    }

    public static Sonuc SakinEkle(string ad, string soyad, string telefon, int daireId, int kullaniciId)
    {
        if (string.IsNullOrWhiteSpace(ad) || string.IsNullOrWhiteSpace(soyad))
        {
            return new Sonuc(false, "Eksik veya hatalı bilgi var. Ad ve soyad zorunludur.");
        }

        if (daireId <= 0 || Veritabani.Sayi("SELECT COUNT(*) FROM Daireler WHERE DaireID = @id", ("@id", daireId)) == 0)
        {
            return new Sonuc(false, "Seçilen daire bulunmuyor. Önce blok ve daire kaydı oluşturun.");
        }

        Veritabani.Calistir(
            @"INSERT INTO Sakinler (DaireID, KullaniciID, Ad, Soyad, Telefon, Aktif)
              VALUES (@daire, @kullanici, @ad, @soyad, @telefon, 1)",
            ("@daire", daireId),
            ("@kullanici", kullaniciId == 0 ? null : kullaniciId),
            ("@ad", ad.Trim()),
            ("@soyad", soyad.Trim()),
            ("@telefon", string.IsNullOrWhiteSpace(telefon) ? null : telefon.Trim()));

        if (kullaniciId > 0)
        {
            Veritabani.Calistir("UPDATE Daireler SET KullaniciID = @k WHERE DaireID = @d", ("@k", kullaniciId), ("@d", daireId));
            Veritabani.Calistir(
                "UPDATE Kullanicilar SET Telefon = COALESCE(Telefon, @t) WHERE KullaniciID = @k",
                ("@t", telefon.Trim()),
                ("@k", kullaniciId));
        }

        return new Sonuc(true, "Sakin kaydı oluşturuldu.");
    }

    public static Sonuc AidatOlustur(int kullaniciId, string donem, decimal tutar, DateTime sonOdeme, IReadOnlyList<int> daireler)
    {
        if (tutar <= 0)
        {
            return new Sonuc(false, "Tutar geçersiz. Sıfırdan büyük bir tutar giriniz.");
        }

        if (daireler.Count == 0)
        {
            return new Sonuc(false, "En az bir daire seçiniz.");
        }

        int eklenen = 0;
        int atlanan = 0;
        using MySqlConnection baglanti = Veritabani.BaglantiAc();
        using MySqlTransaction tx = baglanti.BeginTransaction();

        long donemId;
        using (MySqlCommand ara = new MySqlCommand("SELECT AidatDonemID FROM AidatDonemleri WHERE Donem = @donem", baglanti, tx))
        {
            ara.Parameters.AddWithValue("@donem", donem);
            object? id = ara.ExecuteScalar();
            if (id == null || id == DBNull.Value)
            {
                using MySqlCommand ekle = new MySqlCommand(
                    @"INSERT INTO AidatDonemleri (Donem, Tutar, SonOdemeTarihi, OlusturanKullaniciID)
                      VALUES (@donem, @tutar, @son, @kullanici)", baglanti, tx);
                ekle.Parameters.AddWithValue("@donem", donem);
                ekle.Parameters.AddWithValue("@tutar", tutar);
                ekle.Parameters.AddWithValue("@son", sonOdeme.Date);
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
        string mesaj = eklenen + " daire için aidat borcu oluşturuldu.";
        if (atlanan > 0)
        {
            mesaj += " " + atlanan + " dairede bu dönem zaten kayıtlıydı, mükerrer yazılmadı.";
        }

        return new Sonuc(true, mesaj);
    }

    public static Sonuc OdemeKaydet(int borcId, decimal odeme, DateTime tarih, string yontem, int kullaniciId)
    {
        List<Satir> borc = Veritabani.Satirlar(
            "SELECT Tutar, KalanTutar FROM AidatBorclari WHERE AidatBorcID = @id",
            ("@id", borcId));
        if (borc.Count == 0)
        {
            return new Sonuc(false, "Kayıt bulunamadı.");
        }

        decimal toplam = borc[0].Sayi("Tutar");
        decimal kalan = borc[0].Sayi("KalanTutar");
        if (odeme <= 0 || kalan <= 0)
        {
            return new Sonuc(false, kalan <= 0 ? "Bu borcun kalan tutarı yok." : "Ödeme tutarı geçersiz.");
        }

        if (odeme > kalan)
        {
            return new Sonuc(false, "Ödeme tutarı kalan borçtan büyük.");
        }

        using MySqlConnection baglanti = Veritabani.BaglantiAc();
        using MySqlTransaction tx = baglanti.BeginTransaction();
        using (MySqlCommand ekle = new MySqlCommand(
            @"INSERT INTO Odemeler (AidatBorcID, Tutar, OdemeTarihi, OdemeYontemi, KaydedenKullaniciID)
              VALUES (@borc, @tutar, @tarih, @yontem, @kullanici)", baglanti, tx))
        {
            ekle.Parameters.AddWithValue("@borc", borcId);
            ekle.Parameters.AddWithValue("@tutar", odeme);
            ekle.Parameters.AddWithValue("@tarih", tarih.Date);
            ekle.Parameters.AddWithValue("@yontem", yontem);
            ekle.Parameters.AddWithValue("@kullanici", kullaniciId);
            ekle.ExecuteNonQuery();
        }

        decimal yeniKalan = kalan - odeme;
        using (MySqlCommand guncelle = new MySqlCommand(
            "UPDATE AidatBorclari SET KalanTutar = @kalan, Durum = @durum WHERE AidatBorcID = @id", baglanti, tx))
        {
            guncelle.Parameters.AddWithValue("@kalan", yeniKalan);
            guncelle.Parameters.AddWithValue("@durum", Veritabani.BorcDurumu(yeniKalan, toplam));
            guncelle.Parameters.AddWithValue("@id", borcId);
            guncelle.ExecuteNonQuery();
        }

        tx.Commit();
        return new Sonuc(true, "Ödeme kaydedildi, kalan borç güncellendi.");
    }

    public static Sonuc GiderEkle(int kullaniciId, string? kategori, string aciklama, decimal tutar, DateTime tarih)
    {
        if (string.IsNullOrWhiteSpace(aciklama) || string.IsNullOrWhiteSpace(kategori) || tutar <= 0)
        {
            return new Sonuc(false, "Tutar geçersiz veya zorunlu bilgiler eksik.");
        }

        Veritabani.Calistir(
            @"INSERT INTO Giderler (Kategori, Aciklama, Tutar, Tarih, KaydedenKullaniciID)
              VALUES (@kat, @aciklama, @tutar, @tarih, @kullanici)",
            ("@kat", kategori),
            ("@aciklama", aciklama.Trim()),
            ("@tutar", tutar),
            ("@tarih", tarih.Date),
            ("@kullanici", kullaniciId));
        return new Sonuc(true, "Gider kaydı oluşturuldu.");
    }

    public static void YorumTablosunuHazirla()
    {
        Veritabani.Calistir(
            @"CREATE TABLE IF NOT EXISTS DuyuruYorumlari (
                YorumID INT AUTO_INCREMENT PRIMARY KEY,
                DuyuruID INT NOT NULL,
                KullaniciID INT NOT NULL,
                Yorum TEXT NOT NULL,
                Tarih DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                CONSTRAINT FK_Yorum_Duyuru FOREIGN KEY (DuyuruID) REFERENCES Duyurular (DuyuruID)
                    ON UPDATE CASCADE ON DELETE CASCADE,
                CONSTRAINT FK_Yorum_Kullanici FOREIGN KEY (KullaniciID) REFERENCES Kullanicilar (KullaniciID)
                    ON UPDATE CASCADE ON DELETE CASCADE
              ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4");
    }

    public static Sonuc DuyuruYayimla(int kullaniciId, string baslik, string icerik, DateTime tarih)
    {
        if (string.IsNullOrWhiteSpace(baslik) || string.IsNullOrWhiteSpace(icerik))
        {
            return new Sonuc(false, "Başlık veya içerik eksik.");
        }

        Veritabani.Calistir(
            @"INSERT INTO Duyurular (Baslik, Icerik, YayimTarihi, OlusturanKullaniciID)
              VALUES (@baslik, @icerik, @tarih, @kullanici)",
            ("@baslik", baslik.Trim()),
            ("@icerik", icerik.Trim()),
            ("@tarih", tarih),
            ("@kullanici", kullaniciId));
        return new Sonuc(true, "Duyuru yayımlandı. Tüm roller görüntüleyip yorum yazabilir.");
    }

    public static Sonuc YorumEkle(int duyuruId, int kullaniciId, string yorum)
    {
        if (string.IsNullOrWhiteSpace(yorum))
        {
            return new Sonuc(false, "Yorum boş olamaz.");
        }

        Veritabani.Calistir(
            @"INSERT INTO DuyuruYorumlari (DuyuruID, KullaniciID, Yorum)
              VALUES (@duyuru, @kullanici, @yorum)",
            ("@duyuru", duyuruId),
            ("@kullanici", kullaniciId),
            ("@yorum", yorum.Trim()));
        return new Sonuc(true, "Yorum kaydedildi.");
    }

    public static Sonuc ArizaEkle(int kullaniciId, int daireId, string? alan, string? kategori, string aciklama, string? oncelik)
    {
        if (string.IsNullOrWhiteSpace(aciklama))
        {
            return new Sonuc(false, "Açıklama eksik.");
        }

        List<ListeKalemi> yetkili = Veritabani.KullaniciDaireleri(kullaniciId);
        if (daireId <= 0 || !yetkili.Exists(x => x.Id == daireId))
        {
            return new Sonuc(false, "Yetkili olduğunuz bir daire bulunamadı. Yönetici sizi daireye bağlamadan bildirim açılamaz.");
        }

        Veritabani.Calistir(
            @"INSERT INTO ArizaBildirimleri
              (BildirenKullaniciID, DaireID, Alan, Kategori, Aciklama, Oncelik, Durum)
              VALUES (@kullanici, @daire, @alan, @kategori, @aciklama, @oncelik, @durum)",
            ("@kullanici", kullaniciId),
            ("@daire", daireId),
            ("@alan", alan),
            ("@kategori", kategori),
            ("@aciklama", aciklama.Trim()),
            ("@oncelik", oncelik),
            ("@durum", Veritabani.ArizaBeklemede));
        return new Sonuc(true, "Bildirim kaydedildi. Durum: Beklemede.");
    }

    public static Sonuc ArizaGuncelle(int kullaniciId, string rol, int arizaId, string durum, string cozum)
    {
        bool yetkili = string.Equals(rol, Veritabani.RolYonetici, StringComparison.OrdinalIgnoreCase)
            || string.Equals(rol, Veritabani.RolTeknisyen, StringComparison.OrdinalIgnoreCase);
        if (!yetkili)
        {
            return new Sonuc(false, "Yetkisiz kullanıcıların kayıt değiştirmesine izin verilmez.");
        }

        if (Veritabani.Sayi("SELECT COUNT(*) FROM ArizaBildirimleri WHERE ArizaID = @id", ("@id", arizaId)) == 0)
        {
            return new Sonuc(false, "Kayıt bulunamadı.");
        }

        if (durum != Veritabani.ArizaIslemde && durum != Veritabani.ArizaTamamlandi && durum != Veritabani.ArizaBeklemede)
        {
            return new Sonuc(false, "Geçerli bir durum seçiniz.");
        }

        Veritabani.Calistir(
            @"UPDATE ArizaBildirimleri
              SET Durum = @durum, CozumNotu = @not, GuncellemeTarihi = NOW(), GuncelleyenKullaniciID = @kullanici
              WHERE ArizaID = @id",
            ("@durum", durum),
            ("@not", string.IsNullOrWhiteSpace(cozum) ? null : cozum.Trim()),
            ("@kullanici", kullaniciId),
            ("@id", arizaId));
        return new Sonuc(true, "Arıza durumu güncellendi.");
    }

    public static string BaglantiDene()
    {
        Veritabani.AdminHesabiniHazirla();
        int n = Veritabani.Sayi("SELECT COUNT(*) FROM Kullanicilar");
        return "Bağlantı tamam. Kayıtlı kullanıcı: " + n + ".";
    }

    private static Sonuc Zorunlu(string ad, string soyad, string kullaniciAdi, string sifre, string sifreTekrar)
    {
        if (string.IsNullOrWhiteSpace(ad) || string.IsNullOrWhiteSpace(soyad)
            || string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre)
            || string.IsNullOrWhiteSpace(sifreTekrar))
        {
            return new Sonuc(false, "Lütfen tüm alanları doldurunuz.");
        }

        return new Sonuc(true, "");
    }

    private static Sonuc KullaniciEkle(string ad, string soyad, string kullaniciAdi, string sifre, string rol, string ayrilmisMesaj, string basari)
    {
        if (string.Equals(kullaniciAdi.Trim(), Veritabani.AdminKullaniciAdi, StringComparison.OrdinalIgnoreCase)
            || string.Equals(kullaniciAdi.Trim(), Veritabani.GuvenlikKullaniciAdi, StringComparison.OrdinalIgnoreCase))
        {
            return new Sonuc(false, ayrilmisMesaj);
        }

        try
        {
            if (Veritabani.Sayi("SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi = @kullaniciAdi", ("@kullaniciAdi", kullaniciAdi.Trim())) > 0)
            {
                return new Sonuc(false, "Bu kullanıcı adı zaten kayıtlı.");
            }

            Veritabani.Calistir(
                @"INSERT INTO Kullanicilar (Ad, Soyad, KullaniciAdi, Sifre, RolAdi)
                  VALUES (@ad, @soyad, @kullaniciAdi, @sifre, @rolAdi)",
                ("@ad", ad.Trim()),
                ("@soyad", soyad.Trim()),
                ("@kullaniciAdi", kullaniciAdi.Trim()),
                ("@sifre", SifreKoruma.Ozetle(sifre)),
                ("@rolAdi", rol));
            return new Sonuc(true, basari);
        }
        catch (MySqlException ex) when (ex.Number == 1062)
        {
            return new Sonuc(false, "Bu kullanıcı adı zaten kayıtlı.");
        }
    }
}
