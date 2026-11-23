# Apartman Yönetim Paneli V0.1

Windows Forms ile yazılmış, koyu temalı bir **apartman / site yönetim** paneli.

Blok–daire, sakin, aidat, ödeme, gider, duyuru ve arıza süreçlerini roller üzerinden yönetir.

![Tema](https://img.shields.io/badge/UI-Dark%20%2B%20Lime-c5e11d?style=flat-square)
![Platform](https://img.shields.io/badge/Platform-Windows-0078D4?style=flat-square)
![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=flat-square)
![MySQL](https://img.shields.io/badge/DB-MySQL-4479A1?style=flat-square)

---

## Özellikler

| Modül | Açıklama |
| --- | --- |
| Giriş / Kayıt | Rol bazlı oturum; sakin kaydı herkese açık |
| Admin paneli | Apartman yöneticisi hesabı oluşturma |
| Yönetici paneli | Blok, daire, sakin, aidat, ödeme, gider, duyuru, arıza, personel, kayıtlı araç, bilet ve güvenlik kayıtları |
| Muhasebeci | Ödeme ve gider yönetimi |
| Teknisyen | Arıza / bakım takibi |
| Güvenlik | Ziyaret, kargo, salt okunur sakin rehberi, plaka ve geçici park, olay raporu, acil durum kaydı, yönetim bileti |
| Sakin | Duyurular, aidat bilgisi, arıza bildirimi |
| Dashboard | Analiz kartları + son duyuru / görev listesi |

Arayüz: koyu antrasit arka plan, fıstık yeşili vurgu (`#c5e11d`), sol dikey menü, üstte marka logosu.

---

## Hızlı başlangıç (Release indirme)

1. [Releases](https://github.com/Electusys/apartman-yonetim-paneli/releases) sayfasından son sürümün ZIP dosyasını indirin.
2. ZIP’i bir klasöre çıkarın.
3. Bilgisayarınızda **MySQL** çalışıyor olsun.
4. MySQL’de sırayla `sql/00_kurulum.sql`, `sql/ApartmanYonetimDB_senaryo_tablolari.sql` ve `sql/02_guvenlik.sql` dosyalarını çalıştırın. Demo verisi için isteğe bağlı `sql/01_demo_kullanici_ve_ariza.sql` vardır.
5. Aynı klasördeki `Apartman Yönetim Paneli-V0.1.dll.config` (veya `App.config` çıktısı) içinde MySQL şifrenizi güncelleyin:

```xml
<add name="ApartmanDb"
     connectionString="Server=127.0.0.1;Port=3306;Database=ApartmanYonetimDB;Uid=root;Pwd=SIFRENIZ;" />
```

6. `Apartman Yönetim Paneli-V0.1.exe` dosyasını çalıştırın.

### Varsayılan hesaplar

| Rol | Kullanıcı adı | Şifre |
| --- | --- | --- |
| Admin | `admin` | `1234` |
| Güvenlik | `güvenlik` | `1234` |

Admin şifresi `App.config` / yayınlanan `.dll.config` dosyasından değiştirilebilir. Uygulama açılırken Admin ve güvenlik hesapları yoksa otomatik oluşturulur. Polis ve İtfaiye düğmeleri arama yapmaz; yalnızca kayıt açar.

> **Not:** `win-x64` release paketi **self-contained**dir; ayrı .NET kurulumu gerekmez. MySQL yine gereklidir.

---

## Kaynak koddan çalıştırma

### Gereksinimler

- Windows 10/11
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MySQL 8.x (veya uyumlu)

### Adımlar

```bash
git clone https://github.com/Electusys/apartman-yonetim-paneli.git
cd apartman-yonetim-paneli
```

1. MySQL’de sırayla çalıştırın:
   - `sql/00_kurulum.sql`
   - `sql/ApartmanYonetimDB_senaryo_tablolari.sql`
   - `sql/02_guvenlik.sql`
2. `Apartman Yönetim Paneli-V0.1/App.config` içinde `Pwd=` değerini kendi MySQL şifrenizle doldurun.
3. Derleyip çalıştırın:

```bash
dotnet restore "Apartman Yönetim Paneli-V0.1/Apartman Yönetim Paneli-V0.1.csproj"
dotnet run --project "Apartman Yönetim Paneli-V0.1/Apartman Yönetim Paneli-V0.1.csproj"
```

Visual Studio / VS Code ile `.slnx` veya `.csproj` dosyasını da açabilirsiniz.

---

## Rol hiyerarşisi

```
Admin
 └── Apartman Yöneticisi
      ├── Muhasebeci
      ├── Teknisyen
      ├── Güvenlik
      └── Apartman Sakini (kayıt formu ile de açılabilir)
```

- **Admin** → yönetici hesabı açar  
- **Apartman Yöneticisi** → muhasebeci, teknisyen ve güvenlik atar; operasyonel modülleri yönetir  
- **Güvenlik** → ziyaret, kargo, plaka ve olay kaydı tutar; aidat ve kimlik bilgisi görmez  
- **Sakin** → kendi aidat ve arıza ekranlarını kullanır  

---

## Proje yapısı

```
apartman-yonetim-paneli/
├── Apartman Yönetim Paneli-V0.1/
│   ├── Assets/logo.png            # Marka logosu
│   ├── App.config                 # Bağlantı + Admin ayarları
│   ├── Data/Veritabani.cs         # MySQL erişimi
│   ├── Ui/
│   │   ├── UiTema.cs              # Renkler, buton/kart stilleri
│   │   └── FormIskeleti.cs        # Ortak form iskeleti (sol menü + içerik)
│   └── Forms/
│       ├── Auth/                  # Giriş, kayıt
│       ├── Admin/                 # Admin paneli, personel atama
│       ├── Shell/FrmAnaPanel.cs   # Rol paneli (ana kabuk)
│       ├── Yapi/                  # Blok-daire, sakin
│       ├── Finans/                # Aidat, ödeme, gider
│       ├── Operasyon/             # Duyuru, arıza
│       └── Guvenlik/              # Ziyaret, kargo, plaka, olay, bilet
├── sql/
│   ├── 00_kurulum.sql
│   ├── 01_demo_kullanici_ve_ariza.sql
│   ├── 02_guvenlik.sql
│   └── ApartmanYonetimDB_senaryo_tablolari.sql
└── README.md
```

Her ekran kendi dosyasındadır (`Frm…cs`). Klasör adı modülü gösterir.

---

## Güvenlik notu

Bu sürüm eğitim / demo amaçlıdır. Şifreler düz metin tutulur; üretim ortamı için hash’leme, yetki denetimi ve yapılandırma ayrımı eklenmelidir. Bağlantı bilgilerini kaynak koda gömmeyin; `App.config` üzerinden yönetin.

---

## Lisans

Bu proje örnek / eğitim amaçlı paylaşılmıştır. İstediğiniz gibi inceleyebilir, geliştirebilir ve kendi projelerinizde temel alabilirsiniz.
