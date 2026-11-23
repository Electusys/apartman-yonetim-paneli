namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Yöneticinin, güvenlik personelinin ilettiği biletleri görüntülediği salt okunur liste.
    /// </summary>
    public class FrmBiletListesi : Form
    {
        public FrmBiletListesi(string adSoyad)
        {
            Panel govde = FormIskeleti.Olustur(
                this,
                adSoyad,
                "Yönetim Biletleri",
                "Güvenlik personelinin ilettiği talep ve şikayetler. Kayıtlar oluşturulduğu saat ve kişiyle durur.");

            DataGridView dgv = FormIskeleti.Tablo();
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            govde.Controls.Add(dgv);
            try
            {
                dgv.DataSource = Veritabani.TabloGetir(
                    @"SELECT b.BiletID, b.Konu, b.Aciklama, b.SakinAdSoyad,
                             CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                             CONCAT(k.Ad, ' ', k.Soyad) AS Olusturan,
                             k.KullaniciAdi, b.OlusturmaTarihi
                      FROM YonetimBiletleri b
                      INNER JOIN Kullanicilar k ON k.KullaniciID = b.OlusturanKullaniciID
                      LEFT JOIN Daireler d ON d.DaireID = b.DaireID
                      ORDER BY b.BiletID DESC");
                UiTema.KolonBasliklari(dgv,
                    ("BiletID", "ID"),
                    ("Aciklama", "Açıklama"),
                    ("SakinAdSoyad", "Sakin"),
                    ("Olusturan", "Oluşturan"),
                    ("KullaniciAdi", "Kullanıcı adı"),
                    ("OlusturmaTarihi", "Tarih ve saat"));
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Biletler yüklenemedi");
            }
        }
    }
}
