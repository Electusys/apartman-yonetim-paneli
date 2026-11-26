using System.Data;

namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Ana paneldeki sayaç ve liste sorguları. Ekran bunları yalnızca gösterir.
    /// </summary>
    internal static class PanelOzet
    {
        public static PanelKartlari Guvenlik(int kullaniciId)
        {
            return new PanelKartlari(
                Veritabani.Sayi("SELECT COUNT(*) FROM ZiyaretKayitlari WHERE DATE(GirisZamani) = CURDATE()").ToString(),
                Veritabani.Sayi("SELECT COUNT(*) FROM Kargolar").ToString(),
                Veritabani.Sayi("SELECT COUNT(*) FROM GeciciParkIzinleri WHERE BaslangicZamani <= NOW() AND BitisZamani >= NOW()").ToString(),
                Veritabani.Sayi("SELECT COUNT(*) FROM YonetimBiletleri").ToString(),
                Veritabani.TabloGetir(
                    @"SELECT IslemTuru AS İşlem, DATE_FORMAT(IslemTarihi, '%d.%m.%Y %H:%i') AS Zaman, Aciklama
                      FROM GuvenlikIslemKayitlari
                      WHERE KullaniciID = @id
                      ORDER BY IslemID DESC
                      LIMIT 8",
                    ("@id", kullaniciId)));
        }

        public static PanelKartlari Genel()
        {
            return new PanelKartlari(
                Veritabani.Sayi("SELECT COUNT(*) FROM Bloklar").ToString(),
                Veritabani.Sayi("SELECT COUNT(*) FROM Daireler").ToString(),
                Veritabani.Sayi("SELECT COUNT(*) FROM Sakinler WHERE Aktif = 1").ToString(),
                Veritabani.Sayi("SELECT COUNT(*) FROM ArizaBildirimleri WHERE Durum <> @d", ("@d", Veritabani.ArizaTamamlandi)).ToString(),
                Veritabani.TabloGetir(
                    @"SELECT Baslik AS Görev, DATE_FORMAT(YayimTarihi, '%d.%m.%Y') AS Tarih
                      FROM Duyurular
                      ORDER BY YayimTarihi DESC
                      LIMIT 8"));
        }
    }

    internal sealed class PanelKartlari
    {
        public PanelKartlari(string birinci, string ikinci, string ucuncu, string dorduncu, DataTable liste)
        {
            Birinci = birinci;
            Ikinci = ikinci;
            Ucuncu = ucuncu;
            Dorduncu = dorduncu;
            Liste = liste;
        }

        public string Birinci { get; }
        public string Ikinci { get; }
        public string Ucuncu { get; }
        public string Dorduncu { get; }
        public DataTable Liste { get; }
    }
}
