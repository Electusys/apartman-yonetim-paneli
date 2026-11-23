-- =============================================================================
-- Güvenlik rolü
-- MySQL Workbench: bu dosyanın tamamını seçip Execute (şimşek) ile çalıştırın.
-- Aidat, borç, ödeme ve kimlik tablolarına bağlantı yoktur.
-- Giriş: kullanıcı adı güvenlik / şifre 1234
-- =============================================================================

USE ApartmanYonetimDB;

INSERT IGNORE INTO Roller (RolAdi) VALUES ('Güvenlik');

INSERT INTO Kullanicilar (Ad, Soyad, KullaniciAdi, Sifre, RolAdi, Durum)
SELECT 'Güvenlik', 'Personeli', 'güvenlik', '1234', 'Güvenlik', 'Aktif'
WHERE NOT EXISTS (
    SELECT 1 FROM Kullanicilar WHERE KullaniciAdi = 'güvenlik'
);

UPDATE Kullanicilar
SET Ad = 'Güvenlik',
    Soyad = 'Personeli',
    Sifre = '1234',
    RolAdi = 'Güvenlik',
    Durum = 'Aktif'
WHERE KullaniciAdi = 'güvenlik';

-- -----------------------------------------------------------------------------
-- İşlem günlüğü. Güvenlik personelinin her işlemi burada kalır.
-- Uygulama bu tabloyu güncellemez ve silmez; tetikleyici de engeller.
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS GuvenlikIslemKayitlari (
    IslemID      INT AUTO_INCREMENT PRIMARY KEY,
    KullaniciID  INT          NOT NULL,
    IslemTuru    VARCHAR(40)  NOT NULL,
    Aciklama     VARCHAR(500) NOT NULL,
    IslemTarihi  DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    KEY IX_GuvenlikIslem_Kullanici_Tarih (KullaniciID, IslemTarihi),
    CONSTRAINT FK_GuvenlikIslem_Kullanici
        FOREIGN KEY (KullaniciID) REFERENCES Kullanicilar (KullaniciID)
        ON UPDATE CASCADE ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- -----------------------------------------------------------------------------
-- Misafir, kurye ve araç girişi.
-- DaireID -> Daireler, KaydedenKullaniciID -> Kullanicilar
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS ZiyaretKayitlari (
    ZiyaretID            INT AUTO_INCREMENT PRIMARY KEY,
    Tur                  VARCHAR(20)  NOT NULL COMMENT 'Misafir / Kurye / Arac',
    AdSoyad              VARCHAR(100) NOT NULL,
    DaireID              INT          NOT NULL,
    GirisZamani          DATETIME     NOT NULL,
    CikisZamani          DATETIME     NULL,
    Plaka                VARCHAR(20)  NULL,
    KaydedenKullaniciID  INT          NOT NULL,
    KayitTarihi          DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    KEY IX_Ziyaret_Daire (DaireID),
    KEY IX_Ziyaret_Plaka (Plaka),
    CONSTRAINT FK_Ziyaret_Daire
        FOREIGN KEY (DaireID) REFERENCES Daireler (DaireID)
        ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT FK_Ziyaret_Kullanici
        FOREIGN KEY (KaydedenKullaniciID) REFERENCES Kullanicilar (KullaniciID)
        ON UPDATE CASCADE ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- -----------------------------------------------------------------------------
-- Teslim alınan kargo.
-- DaireID -> Daireler, KaydedenKullaniciID -> Kullanicilar
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Kargolar (
    KargoID              INT AUTO_INCREMENT PRIMARY KEY,
    DaireID              INT          NOT NULL,
    KargoBilgisi         VARCHAR(200) NOT NULL,
    TeslimAlmaZamani     DATETIME     NOT NULL,
    KaydedenKullaniciID  INT          NOT NULL,
    KayitTarihi          DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    KEY IX_Kargo_Daire (DaireID),
    CONSTRAINT FK_Kargo_Daire
        FOREIGN KEY (DaireID) REFERENCES Daireler (DaireID)
        ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT FK_Kargo_Kullanici
        FOREIGN KEY (KaydedenKullaniciID) REFERENCES Kullanicilar (KullaniciID)
        ON UPDATE CASCADE ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- -----------------------------------------------------------------------------
-- İlgili daireye giden bildirim. KargoID -> Kargolar
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS DaireBildirimleri (
    BildirimID           INT AUTO_INCREMENT PRIMARY KEY,
    DaireID              INT          NOT NULL,
    Baslik               VARCHAR(120) NOT NULL,
    Icerik               VARCHAR(500) NOT NULL,
    KargoID              INT          NULL,
    OlusturanKullaniciID INT          NOT NULL,
    OlusturmaTarihi      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    KEY IX_Bildirim_Daire (DaireID),
    CONSTRAINT FK_Bildirim_Daire
        FOREIGN KEY (DaireID) REFERENCES Daireler (DaireID)
        ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT FK_Bildirim_Kargo
        FOREIGN KEY (KargoID) REFERENCES Kargolar (KargoID)
        ON UPDATE CASCADE ON DELETE SET NULL,
    CONSTRAINT FK_Bildirim_Kullanici
        FOREIGN KEY (OlusturanKullaniciID) REFERENCES Kullanicilar (KullaniciID)
        ON UPDATE CASCADE ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- -----------------------------------------------------------------------------
-- Daireye kayıtlı araç. Plaka sorgusu bu tabloya bakar.
-- DaireID -> Daireler. Aidat ve kimlik sütunu yok.
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS KayitliAraclar (
    AracID               INT AUTO_INCREMENT PRIMARY KEY,
    DaireID              INT          NOT NULL,
    Plaka                VARCHAR(20)  NOT NULL,
    KaydedenKullaniciID  INT          NULL,
    KayitTarihi          DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY UK_KayitliAraclar_Plaka (Plaka),
    KEY IX_KayitliArac_Daire (DaireID),
    CONSTRAINT FK_Arac_Daire
        FOREIGN KEY (DaireID) REFERENCES Daireler (DaireID)
        ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT FK_Arac_Kullanici
        FOREIGN KEY (KaydedenKullaniciID) REFERENCES Kullanicilar (KullaniciID)
        ON UPDATE CASCADE ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- -----------------------------------------------------------------------------
-- Misafir araç için geçici park izni. Başlangıç ve bitiş zorunlu.
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS GeciciParkIzinleri (
    IzinID               INT AUTO_INCREMENT PRIMARY KEY,
    Plaka                VARCHAR(20)  NOT NULL,
    DaireID              INT          NOT NULL,
    ZiyaretciAdSoyad     VARCHAR(100) NOT NULL,
    BaslangicZamani      DATETIME     NOT NULL,
    BitisZamani          DATETIME     NOT NULL,
    KaydedenKullaniciID  INT          NOT NULL,
    KayitTarihi          DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    KEY IX_Park_Plaka (Plaka),
    KEY IX_Park_Daire (DaireID),
    CONSTRAINT CK_Park_Zaman CHECK (BitisZamani > BaslangicZamani),
    CONSTRAINT FK_Park_Daire
        FOREIGN KEY (DaireID) REFERENCES Daireler (DaireID)
        ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT FK_Park_Kullanici
        FOREIGN KEY (KaydedenKullaniciID) REFERENCES Kullanicilar (KullaniciID)
        ON UPDATE CASCADE ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- -----------------------------------------------------------------------------
-- Vardiya teslimi ve kural ihlali. Satır sonradan değiştirilemez ve silinemez.
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS OlayKayitlari (
    OlayID               INT AUTO_INCREMENT PRIMARY KEY,
    Tur                  VARCHAR(30)  NOT NULL COMMENT 'Vardiya Teslimi / Kural Ihlali',
    Aciklama             VARCHAR(500) NOT NULL,
    OlusturanKullaniciID INT          NOT NULL,
    OlusturmaTarihi      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    KEY IX_Olay_Kullanici (OlusturanKullaniciID),
    CONSTRAINT FK_Olay_Kullanici
        FOREIGN KEY (OlusturanKullaniciID) REFERENCES Kullanicilar (KullaniciID)
        ON UPDATE CASCADE ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- -----------------------------------------------------------------------------
-- Polis / İtfaiye kağıt kaydı. Dış birime otomatik bildirim gitmez.
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS AcilDurumKayitlari (
    AcilID               INT AUTO_INCREMENT PRIMARY KEY,
    Tur                  VARCHAR(20)  NOT NULL COMMENT 'Polis / Itfaiye',
    NotMetni             VARCHAR(300) NULL,
    OlusturanKullaniciID INT          NOT NULL,
    OlusturmaTarihi      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT FK_Acil_Kullanici
        FOREIGN KEY (OlusturanKullaniciID) REFERENCES Kullanicilar (KullaniciID)
        ON UPDATE CASCADE ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- -----------------------------------------------------------------------------
-- Sakinin talep veya şikayetinin yöneticiye iletilen bileti.
-- DaireID boş olabilir. İçerik aidat veya kimlik taşımaz.
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS YonetimBiletleri (
    BiletID              INT AUTO_INCREMENT PRIMARY KEY,
    Konu                 VARCHAR(120) NOT NULL,
    Aciklama             VARCHAR(500) NOT NULL,
    SakinAdSoyad         VARCHAR(100) NULL,
    DaireID              INT          NULL,
    OlusturanKullaniciID INT          NOT NULL,
    OlusturmaTarihi      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    KEY IX_Bilet_Daire (DaireID),
    CONSTRAINT FK_Bilet_Daire
        FOREIGN KEY (DaireID) REFERENCES Daireler (DaireID)
        ON UPDATE CASCADE ON DELETE SET NULL,
    CONSTRAINT FK_Bilet_Kullanici
        FOREIGN KEY (OlusturanKullaniciID) REFERENCES Kullanicilar (KullaniciID)
        ON UPDATE CASCADE ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- -----------------------------------------------------------------------------
-- Güvenlik rehberi: yalnızca ad, soyad, daire ve iletişim.
-- Aidat, borç, ödeme ve kimlik bu görünümde yoktur.
-- -----------------------------------------------------------------------------
CREATE OR REPLACE VIEW vw_GuvenlikSakinRehberi AS
SELECT
    s.Ad,
    s.Soyad,
    CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
    s.Telefon AS AcilIletisim
FROM Sakinler s
INNER JOIN Daireler d ON d.DaireID = s.DaireID
WHERE s.Aktif = 1;

CREATE OR REPLACE VIEW vw_GuvenlikPlaka AS
SELECT
    a.Plaka,
    CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
    (
        SELECT GROUP_CONCAT(CONCAT(s.Ad, ' ', s.Soyad) ORDER BY s.Ad SEPARATOR ', ')
        FROM Sakinler s
        WHERE s.DaireID = a.DaireID AND s.Aktif = 1
    ) AS Sakinler
FROM KayitliAraclar a
INNER JOIN Daireler d ON d.DaireID = a.DaireID;

-- Olay ve işlem günlüğü satırları sonradan değiştirilemez / silinemez.
DROP TRIGGER IF EXISTS trg_olay_guncelleme_engelle;
DROP TRIGGER IF EXISTS trg_olay_silme_engelle;
DROP TRIGGER IF EXISTS trg_islem_guncelleme_engelle;
DROP TRIGGER IF EXISTS trg_islem_silme_engelle;

DELIMITER $$

CREATE TRIGGER trg_olay_guncelleme_engelle
BEFORE UPDATE ON OlayKayitlari
FOR EACH ROW
BEGIN
    SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Olay kayitlari sonradan degistirilemez.';
END$$

CREATE TRIGGER trg_olay_silme_engelle
BEFORE DELETE ON OlayKayitlari
FOR EACH ROW
BEGIN
    SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Olay kayitlari silinemez.';
END$$

CREATE TRIGGER trg_islem_guncelleme_engelle
BEFORE UPDATE ON GuvenlikIslemKayitlari
FOR EACH ROW
BEGIN
    SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Guvenlik islem kayitlari degistirilemez.';
END$$

CREATE TRIGGER trg_islem_silme_engelle
BEFORE DELETE ON GuvenlikIslemKayitlari
FOR EACH ROW
BEGIN
    SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Guvenlik islem kayitlari silinemez.';
END$$

DELIMITER ;
