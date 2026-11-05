-- =============================================================================
-- Apartman Yönetim Paneli - temel kurulum
-- MySQL Workbench veya mysql CLI ile çalıştırın.
-- =============================================================================

CREATE DATABASE IF NOT EXISTS ApartmanYonetimDB
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_turkish_ci;

USE ApartmanYonetimDB;

CREATE TABLE IF NOT EXISTS Roller (
    RolID   INT AUTO_INCREMENT PRIMARY KEY,
    RolAdi  VARCHAR(50) NOT NULL,
    UNIQUE KEY UK_Roller_RolAdi (RolAdi)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS Kullanicilar (
    KullaniciID   INT AUTO_INCREMENT PRIMARY KEY,
    Ad            VARCHAR(50)  NOT NULL,
    Soyad         VARCHAR(50)  NOT NULL,
    Telefon       VARCHAR(20)  NULL,
    KullaniciAdi  VARCHAR(50)  NOT NULL,
    Sifre         VARCHAR(100) NOT NULL,
    RolAdi        VARCHAR(50)  NOT NULL,
    Durum         VARCHAR(20)  NOT NULL DEFAULT 'Aktif',
    KayitTarihi   DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY UK_Kullanicilar_KullaniciAdi (KullaniciAdi)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Senaryo tabloları için ardından şunu çalıştırın:
--   sql/ApartmanYonetimDB_senaryo_tablolari.sql
