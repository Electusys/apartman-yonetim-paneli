namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Yönetici ve muhasebeci: ortak gider kaydı.
    /// </summary>
    public class FrmGiderYonetimi : Form
    {
        private readonly ComboBox _cmbKategori;
        private readonly TextBox _txtAciklama;
        private readonly NumericUpDown _nudTutar;
        private readonly DateTimePicker _dtpTarih;
        private readonly DataGridView _dgv;
        private readonly int _kullaniciId;

        public FrmGiderYonetimi(int kullaniciId, string adSoyad)
        {
            _kullaniciId = kullaniciId;
            Panel govde = FormIskeleti.Olustur(this, adSoyad, "Gider Yönetimi", "Kategori, açıklama, tutar ve tarih ile ortak gider ekleyin.");

            TableLayoutPanel tablo = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2 };
            tablo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tablo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            _cmbKategori = FormIskeleti.Combo();
            _cmbKategori.Items.AddRange(new object[] { "Elektrik", "Su", "Doğalgaz", "Asansör", "Temizlik", "Bakım", "Diğer" });
            _cmbKategori.SelectedIndex = 0;
            _txtAciklama = FormIskeleti.Metin();
            _nudTutar = FormIskeleti.Para();
            _dtpTarih = FormIskeleti.Tarih();
            Button btnKaydet = FormIskeleti.Birincil("Kaydet");
            btnKaydet.Click += BtnKaydet_Click;

            TableLayoutPanel form = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68F));
            void Satir(string e, Control c)
            {
                int i = form.RowCount++;
                form.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                form.Controls.Add(FormIskeleti.Etiket(e), 0, i);
                form.Controls.Add(c, 1, i);
            }

            Satir("Kategori", _cmbKategori);
            Satir("Açıklama", _txtAciklama);
            Satir("Tutar", _nudTutar);
            Satir("Tarih", _dtpTarih);
            Satir("", btnKaydet);

            _dgv = FormIskeleti.Tablo();
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
                    @"SELECT GiderID, Kategori, Aciklama, Tutar, Tarih, KayitTarihi
                      FROM Giderler
                      ORDER BY GiderID DESC");
                UiTema.KolonBasliklari(_dgv, ("GiderID", "ID"), ("Aciklama", "Açıklama"), ("KayitTarihi", "Kayıt"));
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Gider listesi yüklenemedi");
            }
        }

        private void BtnKaydet_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtAciklama.Text) || _cmbKategori.SelectedIndex < 0)
            {
                FormIskeleti.Uyari("Tutar geçersiz veya zorunlu bilgiler eksik.");
                return;
            }

            if (_nudTutar.Value <= 0)
            {
                FormIskeleti.Uyari("Tutar geçersiz veya zorunlu bilgiler eksik.");
                return;
            }

            try
            {
                Veritabani.Calistir(
                    @"INSERT INTO Giderler (Kategori, Aciklama, Tutar, Tarih, KaydedenKullaniciID)
                      VALUES (@kat, @aciklama, @tutar, @tarih, @kullanici)",
                    ("@kat", _cmbKategori.SelectedItem?.ToString()),
                    ("@aciklama", _txtAciklama.Text.Trim()),
                    ("@tutar", _nudTutar.Value),
                    ("@tarih", _dtpTarih.Value.Date),
                    ("@kullanici", _kullaniciId));
                _txtAciklama.Clear();
                FormIskeleti.Bilgi("Gider kaydı oluşturuldu.");
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Gider kaydedilemedi");
            }
        }
    }
}
