namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Yönetici duyuru yayımlar. Diğer roller yalnızca listeler.
    /// </summary>
    public class FrmDuyurular : Form
    {
        private readonly bool _yonetici;
        private readonly int _kullaniciId;
        private readonly TextBox _txtBaslik;
        private readonly TextBox _txtIcerik;
        private readonly DateTimePicker _dtpYayim;
        private readonly DataGridView _dgv;

        public FrmDuyurular(int kullaniciId, string adSoyad, string rolAdi)
        {
            _kullaniciId = kullaniciId;
            _yonetici = string.Equals(rolAdi, Veritabani.RolYonetici, StringComparison.OrdinalIgnoreCase);
            Panel govde = FormIskeleti.Olustur(this, adSoyad, "Duyuru Yönetimi",
                _yonetici ? "Başlık, içerik ve yayımlanma tarihi ile duyuru oluşturun." : "Apartman duyurularını buradan okuyabilirsiniz.");

            TableLayoutPanel tablo = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2 };
            tablo.RowStyles.Add(new RowStyle(_yonetici ? SizeType.Absolute : SizeType.AutoSize, _yonetici ? 240 : 0));
            tablo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            _txtBaslik = FormIskeleti.Metin();
            _txtIcerik = FormIskeleti.Metin();
            _txtIcerik.Multiline = true;
            _txtIcerik.Height = 80;
            _txtIcerik.Dock = DockStyle.Fill;
            _dtpYayim = FormIskeleti.Tarih();
            _dtpYayim.Format = DateTimePickerFormat.Custom;
            _dtpYayim.CustomFormat = "dd.MM.yyyy HH:mm";
            _dtpYayim.ShowUpDown = true;

            if (_yonetici)
            {
                Button btnYayimla = FormIskeleti.Birincil("Yayımla");
                btnYayimla.Click += BtnYayimla_Click;
                TableLayoutPanel form = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 4 };
                form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
                form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72F));
                form.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
                form.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                form.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
                form.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
                form.Controls.Add(FormIskeleti.Etiket("Başlık"), 0, 0);
                form.Controls.Add(_txtBaslik, 1, 0);
                form.Controls.Add(FormIskeleti.Etiket("İçerik"), 0, 1);
                form.Controls.Add(_txtIcerik, 1, 1);
                form.Controls.Add(FormIskeleti.Etiket("Yayımlanma tarihi"), 0, 2);
                form.Controls.Add(_dtpYayim, 1, 2);
                form.Controls.Add(new Label(), 0, 3);
                form.Controls.Add(btnYayimla, 1, 3);
                tablo.Controls.Add(form, 0, 0);
            }
            else
            {
                tablo.Controls.Add(new Panel { Height = 1 }, 0, 0);
            }

            _dgv = FormIskeleti.Tablo();
            tablo.Controls.Add(_dgv, 0, 1);
            govde.Controls.Add(tablo);
            Yukle();
        }

        private void Yukle()
        {
            try
            {
                _dgv.DataSource = Veritabani.TabloGetir(
                    @"SELECT DuyuruID, Baslik, Icerik, YayimTarihi, KayitTarihi
                      FROM Duyurular
                      WHERE YayimTarihi <= NOW() OR @yonetici = 1
                      ORDER BY YayimTarihi DESC",
                    ("@yonetici", _yonetici ? 1 : 0));
                UiTema.KolonBasliklari(_dgv,
                    ("DuyuruID", "ID"),
                    ("Baslik", "Başlık"),
                    ("Icerik", "İçerik"),
                    ("YayimTarihi", "Yayımlanma"),
                    ("KayitTarihi", "Kayıt"));
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Duyurular yüklenemedi");
            }
        }

        private void BtnYayimla_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtBaslik.Text) || string.IsNullOrWhiteSpace(_txtIcerik.Text))
            {
                FormIskeleti.Uyari("Başlık veya içerik eksik.");
                return;
            }

            try
            {
                Veritabani.Calistir(
                    @"INSERT INTO Duyurular (Baslik, Icerik, YayimTarihi, OlusturanKullaniciID)
                      VALUES (@baslik, @icerik, @tarih, @kullanici)",
                    ("@baslik", _txtBaslik.Text.Trim()),
                    ("@icerik", _txtIcerik.Text.Trim()),
                    ("@tarih", _dtpYayim.Value),
                    ("@kullanici", _kullaniciId));
                _txtBaslik.Clear();
                _txtIcerik.Clear();
                FormIskeleti.Bilgi("Duyuru kaydedildi ve sakinlerin görüntülemesine açıldı.");
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Duyuru yayımlanamadı");
            }
        }
    }
}
