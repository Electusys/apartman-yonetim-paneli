namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Güvenlik işlem günlüğü. Satırlar salt okunurdur; veritabanı değiştirmeyi ve silmeyi reddeder.
    /// </summary>
    public class FrmGuvenlikGunlugu : Form
    {
        public FrmGuvenlikGunlugu(string adSoyad)
        {
            Panel govde = FormIskeleti.Olustur(
                this,
                adSoyad,
                "Güvenlik Kayıtları",
                "Güvenlik personelinin işlemleri tarih, saat ve kullanıcı ile tutulur. Bu kayıtlar değiştirilemez.");

            DataGridView dgv = FormIskeleti.Tablo();
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            govde.Controls.Add(dgv);
            try
            {
                dgv.DataSource = Veritabani.TabloGetir(
                    @"SELECT i.IslemID, i.IslemTarihi,
                             CONCAT(k.Ad, ' ', k.Soyad) AS Personel,
                             k.KullaniciAdi, i.IslemTuru, i.Aciklama
                      FROM GuvenlikIslemKayitlari i
                      INNER JOIN Kullanicilar k ON k.KullaniciID = i.KullaniciID
                      ORDER BY i.IslemID DESC");
                UiTema.KolonBasliklari(dgv,
                    ("IslemID", "ID"),
                    ("IslemTarihi", "Tarih ve saat"),
                    ("KullaniciAdi", "Kullanıcı adı"),
                    ("IslemTuru", "İşlem"),
                    ("Aciklama", "Açıklama"));
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Güvenlik kayıtları yüklenemedi");
            }
        }
    }
}
