namespace Apartman_Yönetim_Paneli_V0._1
{

    /// <summary>
    /// Yönetici duyuru yayımlar. Tüm roller yayımlanmış duyuruları görür ve yorum yazar.
    /// </summary>
    public class FrmDuyurular : Form
    {
        private readonly bool _yonetici;
        private readonly int _kullaniciId;
        private readonly TextBox _txtBaslik;
        private readonly TextBox _txtIcerik;
        private readonly DateTimePicker _dtpYayim;
        private readonly TextBox _txtYorum;
        private readonly DataGridView _dgv;
        private readonly DataGridView _dgvYorum;
        private bool _yukleniyor;

        public FrmDuyurular(int kullaniciId, string adSoyad, string rolAdi)
        {
            _kullaniciId = kullaniciId;
            _yonetici = string.Equals(rolAdi, Veritabani.RolYonetici, StringComparison.OrdinalIgnoreCase);
            string aciklama = _yonetici
                ? "Duyuru yayımlayın. Tüm roller görüntüler ve altına yorum yazabilir."
                : "Yayımlanmış duyuruları okuyun ve altına yorum yazın.";
            Panel govde = FormIskeleti.Olustur(this, adSoyad, _yonetici ? "Duyuru Yönetimi" : "Duyurular", aciklama);

            TableLayoutPanel tablo = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3 };
            tablo.RowStyles.Add(new RowStyle(SizeType.Absolute, _yonetici ? 188F : 0F));
            tablo.RowStyles.Add(new RowStyle(SizeType.Percent, 52F));
            tablo.RowStyles.Add(new RowStyle(SizeType.Percent, 48F));

            _txtBaslik = FormIskeleti.Metin();
            _txtIcerik = FormIskeleti.Metin();
            _txtIcerik.Multiline = true;
            _txtIcerik.Height = 64;
            _txtIcerik.Dock = DockStyle.Fill;
            _dtpYayim = FormIskeleti.Tarih("dd.MM.yyyy HH:mm");

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
            _dgv.SelectionChanged += DuyuruSecildi;
            tablo.Controls.Add(_dgv, 0, 1);

            _dgvYorum = FormIskeleti.Tablo();
            _txtYorum = FormIskeleti.Metin();
            Button btnYorum = FormIskeleti.Birincil("Yorum Yaz");
            btnYorum.Dock = DockStyle.Fill;
            btnYorum.Click += BtnYorum_Click;

            TableLayoutPanel yaz = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            yaz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 78F));
            yaz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            yaz.Controls.Add(_txtYorum, 0, 0);
            yaz.Controls.Add(btnYorum, 1, 0);

            TableLayoutPanel yorumlar = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3, Padding = new Padding(0, 8, 0, 0) };
            yorumlar.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            yorumlar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            yorumlar.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            yorumlar.Controls.Add(FormIskeleti.Etiket("Yorumlar"), 0, 0);
            yorumlar.Controls.Add(_dgvYorum, 0, 1);
            yorumlar.Controls.Add(yaz, 0, 2);
            tablo.Controls.Add(yorumlar, 0, 2);

            govde.Controls.Add(tablo);
            YorumTablosunuHazirla();
            Yukle();
        }

        private static void YorumTablosunuHazirla()
        {
            Veritabani.Calistir(
                @"CREATE TABLE IF NOT EXISTS DuyuruYorumlari (
                    YorumID INT AUTO_INCREMENT PRIMARY KEY,
                    DuyuruID INT NOT NULL,
                    KullaniciID INT NOT NULL,
                    Yorum TEXT NOT NULL,
                    Tarih DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    CONSTRAINT FK_Yorum_Duyuru FOREIGN KEY (DuyuruID) REFERENCES Duyurular (DuyuruID)
                        ON UPDATE CASCADE ON DELETE CASCADE,
                    CONSTRAINT FK_Yorum_Kullanici FOREIGN KEY (KullaniciID) REFERENCES Kullanicilar (KullaniciID)
                        ON UPDATE CASCADE ON DELETE CASCADE
                  ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4");
        }

        private void Yukle()
        {
            try
            {
                _yukleniyor = true;
                _dgv.DataSource = Veritabani.TabloGetir(
                    @"SELECT d.DuyuruID, d.Baslik, d.Icerik,
                             CONCAT(IFNULL(k.Ad, ''), ' ', IFNULL(k.Soyad, '')) AS Yazan,
                             d.YayimTarihi,
                             (SELECT COUNT(*) FROM DuyuruYorumlari y WHERE y.DuyuruID = d.DuyuruID) AS YorumSayisi
                      FROM Duyurular d
                      LEFT JOIN Kullanicilar k ON k.KullaniciID = d.OlusturanKullaniciID
                      WHERE d.YayimTarihi <= NOW() OR @yonetici = 1
                      ORDER BY d.YayimTarihi DESC",
                    ("@yonetici", _yonetici ? 1 : 0));
                UiTema.KolonBasliklari(_dgv,
                    ("DuyuruID", "ID"),
                    ("Baslik", "Başlık"),
                    ("Icerik", "İçerik"),
                    ("Yazan", "Yayımlayan"),
                    ("YayimTarihi", "Yayımlanma"),
                    ("YorumSayisi", "Yorum"));
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Duyurular yüklenemedi");
            }
            finally
            {
                _yukleniyor = false;
            }

            YorumlariYukle();
        }

        private void DuyuruSecildi(object? sender, EventArgs e)
        {
            if (_yukleniyor)
            {
                return;
            }

            YorumlariYukle();
        }

        private int? SeciliDuyuruId()
        {
            if (_dgv.CurrentRow == null || !_dgv.Columns.Contains("DuyuruID"))
            {
                return null;
            }

            object deger = _dgv.CurrentRow.Cells["DuyuruID"].Value;
            if (deger == null || deger == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(deger);
        }

        private void YorumlariYukle()
        {
            int? duyuruId = SeciliDuyuruId();
            if (duyuruId == null)
            {
                _dgvYorum.DataSource = null;
                return;
            }

            try
            {
                _dgvYorum.DataSource = Veritabani.TabloGetir(
                    @"SELECT CONCAT(k.Ad, ' ', k.Soyad) AS Yazan, k.RolAdi AS Rol, y.Yorum, y.Tarih
                      FROM DuyuruYorumlari y
                      INNER JOIN Kullanicilar k ON k.KullaniciID = y.KullaniciID
                      WHERE y.DuyuruID = @id
                      ORDER BY y.Tarih",
                    ("@id", duyuruId.Value));
                UiTema.KolonBasliklari(_dgvYorum,
                    ("Yazan", "Yazan"),
                    ("Rol", "Rol"),
                    ("Yorum", "Yorum"),
                    ("Tarih", "Tarih"));
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Yorumlar yüklenemedi");
            }
        }

        private void BtnYayimla_Click(object? sender, EventArgs e)
        {
            if (!_yonetici)
            {
                return;
            }

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
                FormIskeleti.Bilgi("Duyuru yayımlandı. Tüm roller görüntüleyip yorum yazabilir.");
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Duyuru yayımlanamadı");
            }
        }

        private void BtnYorum_Click(object? sender, EventArgs e)
        {
            int? duyuruId = SeciliDuyuruId();
            if (duyuruId == null)
            {
                FormIskeleti.Uyari("Önce listeden bir duyuru seçin.");
                return;
            }

            if (string.IsNullOrWhiteSpace(_txtYorum.Text))
            {
                FormIskeleti.Uyari("Yorum boş olamaz.");
                return;
            }

            try
            {
                Veritabani.Calistir(
                    @"INSERT INTO DuyuruYorumlari (DuyuruID, KullaniciID, Yorum)
                      VALUES (@duyuru, @kullanici, @yorum)",
                    ("@duyuru", duyuruId.Value),
                    ("@kullanici", _kullaniciId),
                    ("@yorum", _txtYorum.Text.Trim()));
                _txtYorum.Clear();
                YorumlariYukle();
                int? secili = duyuruId;
                Yukle();
                Sec(secili);
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Yorum kaydedilemedi");
            }
        }

        private void Sec(int? duyuruId)
        {
            if (duyuruId == null || !_dgv.Columns.Contains("DuyuruID"))
            {
                return;
            }

            foreach (DataGridViewRow satir in _dgv.Rows)
            {
                if (satir.Cells["DuyuruID"].Value != null
                    && Convert.ToInt32(satir.Cells["DuyuruID"].Value) == duyuruId.Value)
                {
                    satir.Selected = true;
                    _dgv.CurrentCell = satir.Cells["DuyuruID"];
                    break;
                }
            }
        }
    }
}
