-- =============================================================================
-- Apartman Yönetim Paneli - senaryo tabloları
-- MySQL Workbench: bu dosyanın tamamını seçip Execute (şimşek) ile çalıştır.
-- Mevcut Kullanicilar ve Roller tabloları silinmez; üzerine eklenir.
-- =============================================================================

USE ApartmanYonetimDB;

-- -----------------------------------------------------------------------------
-- Giriş senaryosu: pasif hesap engeli + sakin telefonu
-- Bu sütunlar zaten varsa "Duplicate column" hatası alırsan bu iki ALTER'ı atla.
-- -----------------------------------------------------------------------------
ALTER TABLE Kullanicilar
    ADD COLUMN Telefon VARCHAR(20) NULL AFTER Soyad;

ALTER TABLE Kullanicilar
    ADD COLUMN Durum VARCHAR(20) NOT NULL DEFAULT 'Aktif' AFTER RolAdi;

UPDATE Kullanicilar
SET Durum = 'Aktif'
WHERE Durum IS NULL OR Durum = '';

-- -----------------------------------------------------------------------------
-- Tablo 3 — Blok ve Daire Kaydı (aktör: Yönetici)
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Bloklar (
    BlokID          INT AUTO_INCREMENT PRIMARY KEY,
    BlokAdi         VARCHAR(50)  NOT NULL,
    KatSayisi       INT          NULL,
    Aciklama        VARCHAR(255) NULL,
    OlusturmaTarihi DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY UK_Bloklar_BlokAdi (BlokAdi)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS Daireler (
    DaireID         INT AUTO_INCREMENT PRIMARY KEY,
    BlokID          INT          NOT NULL,
    DaireNo         VARCHAR(20)  NOT NULL,
    Kat             INT          NULL,
    OlusturmaTarihi DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT FK_Daireler_Blok
        FOREIGN KEY (BlokID) REFERENCES Bloklar (BlokID)
        ON UPDATE CASCADE ON DELETE RESTRICT,
    -- Aynı blokta aynı daire numarası tekrar edilemez (senaryo madde 9)
    UNIQUE KEY UK_Daireler_Blok_DaireNo (BlokID, DaireNo)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- -----------------------------------------------------------------------------
-- Tablo 4 — Sakin Kaydı (aktör: Yönetici)
-- Sakini daireye bağlar. İstersen aynı kişiyi Kullanicilar ile de eşlersin.
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Sakinler (
    SakinID         INT AUTO_INCREMENT PRIMARY KEY,
    DaireID         INT          NOT NULL,
    KullaniciID     INT          NULL,
    Ad              VARCHAR(50)  NOT NULL,
    Soyad           VARCHAR(50)  NOT NULL,
    Telefon         VARCHAR(20)  NULL,
    Aktif           TINYINT(1)   NOT NULL DEFAULT 1,
    KayitTarihi     DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT FK_Sakinler_Daire
        FOREIGN KEY (DaireID) REFERENCES Daireler (DaireID)
        ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT FK_Sakinler_Kullanici
        FOREIGN KEY (KullaniciID) REFERENCES Kullanicilar (KullaniciID)
        ON UPDATE CASCADE ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- -----------------------------------------------------------------------------
-- Tablo 5 — Aidat Oluşturma (aktör: Yönetici)
-- Önce dönem (ay/yıl + tutar + son ödeme), sonra seçilen dairelere borç satırları.
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS AidatDonemleri (
    AidatDonemID    INT AUTO_INCREMENT PRIMARY KEY,
    Donem           VARCHAR(7)   NOT NULL COMMENT 'Örnek: 2026-10',
    Tutar           DECIMAL(10,2) NOT NULL,
    SonOdemeTarihi  DATE         NOT NULL,
    OlusturanKullaniciID INT     NULL,
    OlusturmaTarihi DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY UK_AidatDonemleri_Donem (Donem),
    CONSTRAINT FK_AidatDonemleri_Kullanici
        FOREIGN KEY (OlusturanKullaniciID) REFERENCES Kullanicilar (KullaniciID)
        ON UPDATE CASCADE ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS AidatBorclari (
    AidatBorcID     INT AUTO_INCREMENT PRIMARY KEY,
    AidatDonemID    INT           NOT NULL,
    DaireID         INT           NOT NULL,
    Tutar           DECIMAL(10,2) NOT NULL,
    KalanTutar      DECIMAL(10,2) NOT NULL,
    Durum           VARCHAR(20)   NOT NULL DEFAULT 'Odenmedi'
                    COMMENT 'Odenmedi / KismiOdendi / Odendi',
    OlusturmaTarihi DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT FK_AidatBorclari_Donem
        FOREIGN KEY (AidatDonemID) REFERENCES AidatDonemleri (AidatDonemID)
        ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT FK_AidatBorclari_Daire
        FOREIGN KEY (DaireID) REFERENCES Daireler (DaireID)
        ON UPDATE CASCADE ON DELETE RESTRICT,
    -- Aynı daire + aynı dönem ikinci kez aidat alamaz (senaryo madde 9)
    UNIQUE KEY UK_AidatBorclari_Donem_Daire (AidatDonemID, DaireID)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- -----------------------------------------------------------------------------
-- Tablo 6 — Aidat Ödeme (aktör: Muhasebeci, Yönetici)
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Odemeler (
    OdemeID         INT AUTO_INCREMENT PRIMARY KEY,
    AidatBorcID     INT           NOT NULL,
    Tutar           DECIMAL(10,2) NOT NULL,
    OdemeTarihi     DATE          NOT NULL,
    OdemeYontemi    VARCHAR(30)   NOT NULL COMMENT 'Nakit / Havale / EFT / Kredi Karti',
    KaydedenKullaniciID INT       NULL,
    KayitTarihi     DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT FK_Odemeler_Borc
        FOREIGN KEY (AidatBorcID) REFERENCES AidatBorclari (AidatBorcID)
        ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT FK_Odemeler_Kullanici
        FOREIGN KEY (KaydedenKullaniciID) REFERENCES Kullanicilar (KullaniciID)
        ON UPDATE CASCADE ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- -----------------------------------------------------------------------------
-- Tablo 7 — Ortak Gider Kaydı (aktör: Yönetici, Muhasebeci)
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Giderler (
    GiderID         INT AUTO_INCREMENT PRIMARY KEY,
    Kategori        VARCHAR(50)   NOT NULL COMMENT 'Elektrik / Su / Asansor / Temizlik / Diger',
    Aciklama        VARCHAR(255)  NOT NULL,
    Tutar           DECIMAL(10,2) NOT NULL,
    Tarih           DATE          NOT NULL,
    KaydedenKullaniciID INT       NULL,
    KayitTarihi     DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT FK_Giderler_Kullanici
        FOREIGN KEY (KaydedenKullaniciID) REFERENCES Kullanicilar (KullaniciID)
        ON UPDATE CASCADE ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- -----------------------------------------------------------------------------
-- Tablo 8 — Duyuru Oluşturma (aktör: Yönetici)
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Duyurular (
    DuyuruID        INT AUTO_INCREMENT PRIMARY KEY,
    Baslik          VARCHAR(120)  NOT NULL,
    Icerik          TEXT          NOT NULL,
    YayimTarihi     DATETIME      NOT NULL,
    OlusturanKullaniciID INT      NULL,
    KayitTarihi     DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT FK_Duyurular_Kullanici
        FOREIGN KEY (OlusturanKullaniciID) REFERENCES Kullanicilar (KullaniciID)
        ON UPDATE CASCADE ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- -----------------------------------------------------------------------------
-- Tablo 9-10 — Arıza bildirimi ve durum güncelleme
-- Durum: Beklemede / Islem Yapiliyor / Tamamlandi
-- -----------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS ArizaBildirimleri (
    ArizaID         INT AUTO_INCREMENT PRIMARY KEY,
    BildirenKullaniciID INT       NOT NULL,
    DaireID         INT           NOT NULL,
    Alan            VARCHAR(80)   NOT NULL COMMENT 'Daire ici / Merdiven / Asansor / Otopark ...',
    Kategori        VARCHAR(50)   NOT NULL COMMENT 'Tesisat / Elektrik / Asansor / Diger',
    Aciklama        TEXT          NOT NULL,
    Oncelik         VARCHAR(20)   NOT NULL DEFAULT 'Normal' COMMENT 'Dusuk / Normal / Yuksek',
    Durum           VARCHAR(30)   NOT NULL DEFAULT 'Beklemede',
    CozumNotu       TEXT          NULL,
    OlusturmaTarihi DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    GuncellemeTarihi DATETIME     NULL,
    GuncelleyenKullaniciID INT    NULL,
    CONSTRAINT FK_Ariza_Bildiren
        FOREIGN KEY (BildirenKullaniciID) REFERENCES Kullanicilar (KullaniciID)
        ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT FK_Ariza_Daire
        FOREIGN KEY (DaireID) REFERENCES Daireler (DaireID)
        ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT FK_Ariza_Guncelleyen
        FOREIGN KEY (GuncelleyenKullaniciID) REFERENCES Kullanicilar (KullaniciID)
        ON UPDATE CASCADE ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
