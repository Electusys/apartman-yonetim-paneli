namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Vardiya teslimi ve kural ihlali. Kayıtlar tarih damgalıdır; değiştirilmez ve silinmez.
    /// </summary>
    public class FrmOlayRaporu : Form
    {
        private readonly int _kullaniciId;
        private readonly ComboBox _cmbTur;
        private readonly TextBox _txtAciklama;
        private readonly DataGridView _dgv;

        public FrmOlayRaporu(int kullaniciId, string adSoyad)
        {
            _kullaniciId = kullaniciId;
            Panel govde = FormIskeleti.Olustur(
                this,
                adSoyad,
                "Olay Raporu",
                "Vardiya teslimi veya kural ihlalini yazın. Saat sistem tarafından basılır; kayıt sonra değiştirilemez ve silinemez.");

            _cmbTur = FormIskeleti.Combo();
            _cmbTur.Items.AddRange(new object[] { "Vardiya Teslimi", "Kural İhlali" });
            _cmbTur.SelectedIndex = 0;
            _txtAciklama = FormIskeleti.Metin();
            _txtAciklama.MaxLength = 500;
            Button btnKaydet = FormIskeleti.Birincil("Raporu Kaydet");
            btnKaydet.Click += BtnKaydet_Click;
            Label not = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = UiTema.YaziSoluk,
                Text = "Bu listedeki satırlar salt okunurdur."
            };

            TableLayoutPanel form = GuvenlikOrtak.Alan();
            GuvenlikOrtak.Satir(form, "Tür", _cmbTur);
            GuvenlikOrtak.Satir(form, "Açıklama", _txtAciklama);
            GuvenlikOrtak.Satir(form, "", btnKaydet);
            GuvenlikOrtak.Satir(form, "", not);

            _dgv = FormIskeleti.Tablo();
            _dgv.ReadOnly = true;
            _dgv.AllowUserToAddRows = false;
            _dgv.AllowUserToDeleteRows = false;
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
                _dgv.DataSource = Veritabani.TabloGetir(
                    @"SELECT o.OlayID, o.Tur, o.Aciklama, o.OlusturmaTarihi,
                             CONCAT(k.Ad, ' ', k.Soyad) AS Personel
                      FROM OlayKayitlari o
                      INNER JOIN Kullanicilar k ON k.KullaniciID = o.OlusturanKullaniciID
                      ORDER BY o.OlayID DESC");
                UiTema.KolonBasliklari(_dgv,
                    ("OlayID", "ID"),
                    ("Aciklama", "Açıklama"),
                    ("OlusturmaTarihi", "Tarih ve saat"));
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Olay kayıtları yüklenemedi");
            }
        }

        private void BtnKaydet_Click(object? sender, EventArgs e)
        {
            string aciklama = _txtAciklama.Text.Trim();
            if (string.IsNullOrWhiteSpace(aciklama) || _cmbTur.SelectedIndex < 0)
            {
                FormIskeleti.Uyari("Tür ve açıklama zorunludur.");
                return;
            }

            string tur = _cmbTur.SelectedItem?.ToString() ?? "Vardiya Teslimi";
            try
            {
                Veritabani.Calistir(
                    @"INSERT INTO OlayKayitlari (Tur, Aciklama, OlusturanKullaniciID)
                      VALUES (@tur, @aciklama, @kullanici)",
                    ("@tur", tur),
                    ("@aciklama", aciklama),
                    ("@kullanici", _kullaniciId));
                GuvenlikOrtak.Yaz(_kullaniciId, "Olay raporu", tur + " kaydı oluşturuldu.");
                _txtAciklama.Clear();
                FormIskeleti.Bilgi("Olay kaydı oluşturuldu. Bu kayıt değiştirilemez.");
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Olay kaydı oluşturulamadı");
            }
        }
    }
}
