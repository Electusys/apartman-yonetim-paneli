-- =============================================================================
-- Demo kullanıcılar + sahte arıza kayıtları
-- Tüm roller: kullanıcı adı meslek adı, şifre 1234
-- =============================================================================

USE ApartmanYonetimDB;

-- Bağımlı kayıtları temizle
DELETE FROM ArizaBildirimleri;
DELETE FROM ArizaTalepleri;
DELETE FROM Odemeler;
DELETE FROM AidatBorclari;
DELETE FROM AidatDonemleri;
DELETE FROM Giderler;
DELETE FROM Duyurular;
DELETE FROM Sakinler;
UPDATE Daireler SET KullaniciID = NULL;
DELETE FROM Kullanicilar;

-- Roller (yoksa ekle)
INSERT IGNORE INTO Roller (RolAdi) VALUES
('Admin'),
('Apartman Yöneticisi'),
('Muhasebeci'),
('Teknisyen'),
('Apartman Sakini');

-- Kullanıcılar: kullanıcı adı = meslek, şifre = 1234
INSERT INTO Kullanicilar (Ad, Soyad, KullaniciAdi, Sifre, RolAdi) VALUES
('Sistem', 'Yöneticisi', 'admin', '1234', 'Admin'),
('Apartman', 'Yöneticisi', 'yonetici', '1234', 'Apartman Yöneticisi'),
('Muhasebe', 'Personeli', 'muhasebe', '1234', 'Muhasebeci'),
('Teknik', 'Personel', 'teknisyen', '1234', 'Teknisyen'),
('Apartman', 'Sakini', 'sakin', '1234', 'Apartman Sakini');

SET @admin     := (SELECT KullaniciID FROM Kullanicilar WHERE KullaniciAdi = 'admin');
SET @yonetici  := (SELECT KullaniciID FROM Kullanicilar WHERE KullaniciAdi = 'yonetici');
SET @muhasebe  := (SELECT KullaniciID FROM Kullanicilar WHERE KullaniciAdi = 'muhasebe');
SET @teknisyen := (SELECT KullaniciID FROM Kullanicilar WHERE KullaniciAdi = 'teknisyen');
SET @sakin     := (SELECT KullaniciID FROM Kullanicilar WHERE KullaniciAdi = 'sakin');

-- Örnek daireler (sakin A/1'e bağlı)
DELETE FROM Daireler;
INSERT INTO Daireler (Blok, DaireNo, Kat, KullaniciID) VALUES
('A', '1', 1, @sakin),
('A', '2', 1, NULL),
('A', '5', 2, NULL),
('B', '3', 1, NULL),
('B', '8', 3, NULL);

SET @daireA1 := (SELECT DaireID FROM Daireler WHERE Blok = 'A' AND DaireNo = '1');
SET @daireA2 := (SELECT DaireID FROM Daireler WHERE Blok = 'A' AND DaireNo = '2');
SET @daireA5 := (SELECT DaireID FROM Daireler WHERE Blok = 'A' AND DaireNo = '5');
SET @daireB3 := (SELECT DaireID FROM Daireler WHERE Blok = 'B' AND DaireNo = '3');
SET @daireB8 := (SELECT DaireID FROM Daireler WHERE Blok = 'B' AND DaireNo = '8');

INSERT INTO Sakinler (DaireID, KullaniciID, Ad, Soyad, Telefon, Aktif) VALUES
(@daireA1, @sakin, 'Apartman', 'Sakini', '0555 111 22 33', 1);

-- Sahte arıza bildirimleri
INSERT INTO ArizaBildirimleri
    (BildirenKullaniciID, DaireID, Alan, Kategori, Aciklama, Oncelik, Durum, CozumNotu, OlusturmaTarihi, GuncellemeTarihi, GuncelleyenKullaniciID)
VALUES
(@sakin, @daireA1, 'Daire içi', 'Tesisat',
 'Mutfak lavabosu tıkanık, su tahliye olmuyor.', 'Yüksek', 'Beklemede',
 NULL, DATE_SUB(NOW(), INTERVAL 2 DAY), NULL, NULL),

(@sakin, @daireA1, 'Daire içi', 'Elektrik',
 'Salondaki prize ara sıra elektrik gelmiyor.', 'Normal', 'İşlem Yapılıyor',
 'Sigorta kutusu kontrol edildi, parça bekleniyor.', DATE_SUB(NOW(), INTERVAL 5 DAY), DATE_SUB(NOW(), INTERVAL 1 DAY), @teknisyen),

(@sakin, @daireA5, 'Asansör', 'Asansör',
 'A blok asansörü 2. katta sıkışıp kalıyor.', 'Yüksek', 'İşlem Yapılıyor',
 'Servis firması randevu verdi.', DATE_SUB(NOW(), INTERVAL 3 DAY), NOW(), @yonetici),

(@sakin, @daireB3, 'Merdiven', 'Diğer',
 'B blok merdiven aydınlatması yanmıyor.', 'Düşük', 'Beklemede',
 NULL, DATE_SUB(NOW(), INTERVAL 1 DAY), NULL, NULL),

(@sakin, @daireB8, 'Otopark', 'Elektrik',
 'Otopark giriş sensörü çalışmıyor, kapı açık kalıyor.', 'Yüksek', 'Beklemede',
 NULL, NOW(), NULL, NULL),

(@sakin, @daireA2, 'Çatı', 'Tesisat',
 'Yağmur sonrası A2 tavanında nem lekesi oluştu.', 'Normal', 'Tamamlandı',
 'Çatı izolasyonu yenilendi, sızıntı giderildi.', DATE_SUB(NOW(), INTERVAL 12 DAY), DATE_SUB(NOW(), INTERVAL 4 DAY), @teknisyen),

(@sakin, @daireA1, 'Bahçe', 'Diğer',
 'Bahçe musluğu damlatıyor, su kaybı var.', 'Düşük', 'Tamamlandı',
 'Conta değiştirildi.', DATE_SUB(NOW(), INTERVAL 8 DAY), DATE_SUB(NOW(), INTERVAL 6 DAY), @teknisyen);

SELECT KullaniciID, KullaniciAdi, Sifre, RolAdi FROM Kullanicilar ORDER BY KullaniciID;
SELECT ArizaID, Alan, Kategori, Oncelik, Durum, LEFT(Aciklama, 50) AS Aciklama FROM ArizaBildirimleri ORDER BY ArizaID;
