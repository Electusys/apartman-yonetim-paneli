namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Teslim alınan kargoyu kaydeder ve ilgili daireye bildirim açar.
    /// </summary>
    public class FrmKargoKaydi : Form
    {
        private readonly int _kullaniciId;
        private readonly ComboBox _cmbDaire;
        private readonly TextBox _txtBilgi;
        private readonly DateTimePicker _dtpTeslim;
        private readonly DataGridView _dgv;

        public FrmKargoKaydi(int kullaniciId, string adSoyad)
        {
            _kullaniciId = kullaniciId;
            Panel govde = FormIskeleti.Olustur(
                this,
                adSoyad,
                "Kargo Kaydı",
                "Kargo bilgisi ve daire yeterlidir. Kayıt, ilgili daireye bildirim olarak da yazılır.");

            _cmbDaire = FormIskeleti.Combo();
            _txtBilgi = FormIskeleti.Metin();
            _txtBilgi.MaxLength = 200;
            _dtpTeslim = FormIskeleti.Tarih("dd.MM.yyyy HH:mm");
            _dtpTeslim.Value = DateTime.Now;
            Button btnKaydet = FormIskeleti.Birincil("Kaydet ve daireye bildir");
            btnKaydet.Click += BtnKaydet_Click;

            TableLayoutPanel form = GuvenlikOrtak.Alan();
            GuvenlikOrtak.Satir(form, "Daire", _cmbDaire);
            GuvenlikOrtak.Satir(form, "Kargo bilgisi", _txtBilgi);
            GuvenlikOrtak.Satir(form, "Teslim alma zamanı", _dtpTeslim);
            GuvenlikOrtak.Satir(form, "", btnKaydet);

            _dgv = FormIskeleti.Tablo();
            TableLayoutPanel tablo = GuvenlikOrtak.IkiBolum();
            tablo.Controls.Add(form, 0, 0);
            tablo.Controls.Add(_dgv, 0, 1);
            govde.Controls.Add(tablo);
            Yukle();
        }

        private void Yukle()
        {
            try
            {
                GuvenlikOrtak.DaireCombo(_cmbDaire, false);
                _dgv.DataSource = Veritabani.TabloGetir(
                    @"SELECT k.KargoID,
                             CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                             k.KargoBilgisi, k.TeslimAlmaZamani, b.Baslik AS Bildirim
                      FROM Kargolar k
                      INNER JOIN Daireler d ON d.DaireID = k.DaireID
                      LEFT JOIN DaireBildirimleri b ON b.KargoID = k.KargoID
                      ORDER BY k.KargoID DESC");
                UiTema.KolonBasliklari(_dgv,
                    ("KargoID", "ID"),
                    ("KargoBilgisi", "Kargo bilgisi"),
                    ("TeslimAlmaZamani", "Teslim"));
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Kargo listesi yüklenemedi");
            }
        }

        private void BtnKaydet_Click(object? sender, EventArgs e)
        {
            string bilgi = _txtBilgi.Text.Trim();
            if (string.IsNullOrWhiteSpace(bilgi))
            {
                FormIskeleti.Uyari("Kargo bilgisi zorunludur.");
                return;
            }

            if (GuvenlikOrtak.DaireId(_cmbDaire) is not int daireId)
            {
                FormIskeleti.Uyari("Kargonun bırakılacağı daireyi seçin.");
                return;
            }

            try
            {
                KargoKaydi.Ekle(daireId, bilgi, _dtpTeslim.Value, _kullaniciId);
                _txtBilgi.Clear();
                FormIskeleti.Bilgi("Kargo kaydedildi ve daire bildirimi oluşturuldu.");
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Kargo kaydı oluşturulamadı");
            }
        }
    }
}
