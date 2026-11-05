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


    /// <summary>
    /// Sakin yeni bildirim açar (kendi dairesi).
    /// Teknisyen ve yönetici durum günceller.
    /// </summary>
    public class FrmAriza : Form
    {
        private readonly int _kullaniciId;
        private readonly string _rolAdi;
        private readonly bool _sakin;
        private readonly bool _guncelleyebilir;
        private readonly ComboBox _cmbDaire;
        private readonly ComboBox _cmbAlan;
        private readonly ComboBox _cmbKategori;
        private readonly ComboBox _cmbOncelik;
        private readonly TextBox _txtAciklama;
        private readonly ComboBox _cmbDurum;
        private readonly TextBox _txtCozum;
        private readonly DataGridView _dgv;

        public FrmAriza(int kullaniciId, string adSoyad, string rolAdi)
        {
            _kullaniciId = kullaniciId;
            _rolAdi = rolAdi;
            _sakin = string.Equals(rolAdi, Veritabani.RolSakin, StringComparison.OrdinalIgnoreCase);
            _guncelleyebilir = string.Equals(rolAdi, Veritabani.RolYonetici, StringComparison.OrdinalIgnoreCase)
                || string.Equals(rolAdi, Veritabani.RolTeknisyen, StringComparison.OrdinalIgnoreCase);

            string aciklama = _sakin
                ? "Arıza alanını, kategorisini ve açıklamasını girerek bildirim oluşturun."
                : "Bildirimi seçip durumunu ve çözüm notunu güncelleyin.";
            Panel govde = FormIskeleti.Olustur(this, adSoyad, _sakin ? "Arıza ve Bakım Bildirimleri" : "Arıza ve Bakım Yönetimi", aciklama);

            TableLayoutPanel tablo = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2 };
            tablo.RowStyles.Add(new RowStyle(SizeType.Absolute, 260F));
            tablo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            TableLayoutPanel form = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

            _cmbDaire = FormIskeleti.Combo();
            _cmbAlan = FormIskeleti.Combo();
            _cmbAlan.Items.AddRange(new object[] { "Daire içi", "Merdiven", "Asansör", "Otopark", "Çatı", "Bahçe", "Diğer" });
            _cmbAlan.SelectedIndex = 0;
            _cmbKategori = FormIskeleti.Combo();
            _cmbKategori.Items.AddRange(new object[] { "Tesisat", "Elektrik", "Asansör", "Doğalgaz", "Diğer" });
            _cmbKategori.SelectedIndex = 0;
            _cmbOncelik = FormIskeleti.Combo();
            _cmbOncelik.Items.AddRange(new object[] { "Düşük", "Normal", "Yüksek" });
            _cmbOncelik.SelectedIndex = 1;
            _txtAciklama = FormIskeleti.Metin();
            _cmbDurum = FormIskeleti.Combo();
            _cmbDurum.Items.AddRange(new object[] { Veritabani.ArizaBeklemede, Veritabani.ArizaIslemde, Veritabani.ArizaTamamlandi });
            _cmbDurum.SelectedIndex = 1;
            _txtCozum = FormIskeleti.Metin();

            int satir = 0;
            void SatirEkle(string e, Control c)
            {
                form.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
                form.Controls.Add(FormIskeleti.Etiket(e), 0, satir);
                form.Controls.Add(c, 1, satir);
                satir++;
            }

            if (_sakin)
            {
                SatirEkle("Daire", _cmbDaire);
                SatirEkle("Alan", _cmbAlan);
                SatirEkle("Kategori", _cmbKategori);
                SatirEkle("Öncelik", _cmbOncelik);
                SatirEkle("Açıklama", _txtAciklama);
                Button btnKaydet = FormIskeleti.Birincil("Bildirimi Kaydet");
                btnKaydet.Click += BtnKaydet_Click;
                SatirEkle("", btnKaydet);
            }
            else
            {
                SatirEkle("Durum", _cmbDurum);
                SatirEkle("Çözüm notu", _txtCozum);
                Button btnGuncelle = FormIskeleti.Birincil("Durumu Güncelle");
                btnGuncelle.Enabled = _guncelleyebilir;
                btnGuncelle.Click += BtnGuncelle_Click;
                SatirEkle("", btnGuncelle);
            }

            _dgv = FormIskeleti.Tablo();
            tablo.Controls.Add(form, 0, 0);
            tablo.Controls.Add(_dgv, 0, 1);
            govde.Controls.Add(tablo);
            DaireleriDoldur();
            Yukle();
        }

        private void DaireleriDoldur()
        {
            _cmbDaire.Items.Clear();
            foreach (ListeKalemi kalem in Veritabani.KullaniciDaireleri(_kullaniciId))
            {
                _cmbDaire.Items.Add(kalem);
            }

            if (_cmbDaire.Items.Count > 0)
            {
                _cmbDaire.SelectedIndex = 0;
            }
        }

        private void Yukle()
        {
            try
            {
                string sql = @"SELECT a.ArizaID, CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                                      CONCAT(k.Ad, ' ', k.Soyad) AS Bildiren, a.Alan, a.Kategori, a.Aciklama,
                                      a.Oncelik, a.Durum, a.CozumNotu, a.OlusturmaTarihi
                               FROM ArizaBildirimleri a
                               INNER JOIN Daireler d ON d.DaireID = a.DaireID
                               INNER JOIN Kullanicilar k ON k.KullaniciID = a.BildirenKullaniciID";
                if (_sakin)
                {
                    sql += " WHERE a.BildirenKullaniciID = @id";
                    _dgv.DataSource = Veritabani.TabloGetir(sql + " ORDER BY a.ArizaID DESC", ("@id", _kullaniciId));
                }
                else
                {
                    _dgv.DataSource = Veritabani.TabloGetir(sql + " ORDER BY a.ArizaID DESC");
                }

                UiTema.KolonBasliklari(_dgv,
                    ("ArizaID", "ID"),
                    ("Aciklama", "Açıklama"),
                    ("Oncelik", "Öncelik"),
                    ("CozumNotu", "Çözüm"),
                    ("OlusturmaTarihi", "Tarih"));
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Arıza listesi yüklenemedi");
            }
        }

        private void BtnKaydet_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtAciklama.Text))
            {
                FormIskeleti.Uyari("Açıklama eksik.");
                return;
            }

            if (_cmbDaire.SelectedItem is not ListeKalemi daire)
            {
                FormIskeleti.Uyari("Yetkili olduğunuz bir daire bulunamadı. Yönetici sizi daireye bağlamadan bildirim açılamaz.");
                return;
            }

            List<ListeKalemi> yetkili = Veritabani.KullaniciDaireleri(_kullaniciId);
            if (!yetkili.Exists(x => x.Id == daire.Id))
            {
                FormIskeleti.Uyari("Yetkili olmadığınız bir daire için bildirim oluşturamazsınız.");
                return;
            }

            try
            {
                Veritabani.Calistir(
                    @"INSERT INTO ArizaBildirimleri
                      (BildirenKullaniciID, DaireID, Alan, Kategori, Aciklama, Oncelik, Durum)
                      VALUES (@kullanici, @daire, @alan, @kategori, @aciklama, @oncelik, @durum)",
                    ("@kullanici", _kullaniciId),
                    ("@daire", daire.Id),
                    ("@alan", _cmbAlan.SelectedItem?.ToString()),
                    ("@kategori", _cmbKategori.SelectedItem?.ToString()),
                    ("@aciklama", _txtAciklama.Text.Trim()),
                    ("@oncelik", _cmbOncelik.SelectedItem?.ToString()),
                    ("@durum", Veritabani.ArizaBeklemede));
                _txtAciklama.Clear();
                FormIskeleti.Bilgi("Bildirim kaydedildi. Durum: Beklemede.");
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Bildirim kaydedilemedi");
            }
        }

        private void BtnGuncelle_Click(object? sender, EventArgs e)
        {
            if (!_guncelleyebilir)
            {
                FormIskeleti.Uyari("Yetkisiz kullanıcıların kayıt değiştirmesine izin verilmez.");
                return;
            }

            if (_dgv.CurrentRow == null)
            {
                FormIskeleti.Uyari("Kayıt bulunamadı. Listeden bir arıza seçiniz.");
                return;
            }

            int arizaId = Convert.ToInt32(_dgv.CurrentRow.Cells["ArizaID"].Value);
            if (Veritabani.Sayi("SELECT COUNT(*) FROM ArizaBildirimleri WHERE ArizaID = @id", ("@id", arizaId)) == 0)
            {
                FormIskeleti.Uyari("Kayıt bulunamadı.");
                return;
            }

            string durum = _cmbDurum.SelectedItem?.ToString() ?? Veritabani.ArizaIslemde;
            if (durum != Veritabani.ArizaIslemde && durum != Veritabani.ArizaTamamlandi && durum != Veritabani.ArizaBeklemede)
            {
                FormIskeleti.Uyari("Geçerli bir durum seçiniz.");
                return;
            }

            try
            {
                Veritabani.Calistir(
                    @"UPDATE ArizaBildirimleri
                      SET Durum = @durum, CozumNotu = @not, GuncellemeTarihi = NOW(), GuncelleyenKullaniciID = @kullanici
                      WHERE ArizaID = @id",
                    ("@durum", durum),
                    ("@not", string.IsNullOrWhiteSpace(_txtCozum.Text) ? null : _txtCozum.Text.Trim()),
                    ("@kullanici", _kullaniciId),
                    ("@id", arizaId));
                FormIskeleti.Bilgi("Arıza durumu güncellendi.");
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Güncelleme kaydedilemedi");
            }
        }
    }

}
