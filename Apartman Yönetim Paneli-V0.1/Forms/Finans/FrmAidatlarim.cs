namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Sakin: bağlı olduğu dairelerin aidat borçlarını görür.
    /// </summary>
    public class FrmAidatlarim : Form
    {
        public FrmAidatlarim(int kullaniciId, string adSoyad)
        {
            Panel govde = FormIskeleti.Olustur(this, adSoyad, "Aidat Bilgilerim", "Dairenize tanımlı aidat borçlarını ve kalan tutarı izleyin.");
            DataGridView dgv = FormIskeleti.Tablo();
            govde.Controls.Add(dgv);

            try
            {
                dgv.DataSource = Veritabani.TabloGetir(
                    @"SELECT CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire, dnm.Donem,
                             b.Tutar, b.KalanTutar, b.Durum, dnm.SonOdemeTarihi
                      FROM AidatBorclari b
                      INNER JOIN Daireler d ON d.DaireID = b.DaireID
                      INNER JOIN AidatDonemleri dnm ON dnm.AidatDonemID = b.AidatDonemID
                      WHERE d.DaireID IN (
                          SELECT DaireID FROM Sakinler WHERE KullaniciID = @id AND Aktif = 1
                          UNION
                          SELECT DaireID FROM Daireler WHERE KullaniciID = @id
                      )
                      ORDER BY dnm.Donem DESC",
                    ("@id", kullaniciId));
                UiTema.KolonBasliklari(dgv, ("KalanTutar", "Kalan"), ("SonOdemeTarihi", "Son ödeme"));
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Aidat bilgileri yüklenemedi");
            }
        }
    }
}
