namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Yönetici: sakini daireye bağlar. Daire yoksa kayıt tamamlanmaz.
    /// İsteğe bağlı olarak mevcut sakin kullanıcısı seçilebilir.
    /// </summary>
    public class FrmSakinYonetimi : Form
    {
        private readonly TextBox _txtAd;
        private readonly TextBox _txtSoyad;
        private readonly TextBox _txtTelefon;
        private readonly ComboBox _cmbDaire;
        private readonly ComboBox _cmbKullanici;
        private readonly DataGridView _dgv;

        public FrmSakinYonetimi(string adSoyad)
        {
            Panel govde = FormIskeleti.Olustur(this, adSoyad, "Sakin Yönetimi", "Daire seçerek yeni sakin kaydı oluşturun.");

            TableLayoutPanel tablo = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
            tablo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tablo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            _txtAd = FormIskeleti.Metin();
            _txtSoyad = FormIskeleti.Metin();
            _txtTelefon = FormIskeleti.Metin();
            _cmbDaire = FormIskeleti.Combo();
            _cmbKullanici = FormIskeleti.Combo();
            Button btnEkle = FormIskeleti.Birincil("Yeni Sakin Ekle");
            btnEkle.Click += BtnEkle_Click;

            TableLayoutPanel form = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68F));
            Ekle(form, "Ad", _txtAd);
            Ekle(form, "Soyad", _txtSoyad);
            Ekle(form, "Telefon", _txtTelefon);
            Ekle(form, "Daire", _cmbDaire);
            Ekle(form, "Giriş hesabı (isteğe bağlı)", _cmbKullanici);
            Ekle(form, "", btnEkle);

            _dgv = FormIskeleti.Tablo();
            tablo.Controls.Add(form, 0, 0);
            tablo.Controls.Add(_dgv, 0, 1);
            govde.Controls.Add(tablo);
            Yukle();
        }

        private static void Ekle(TableLayoutPanel form, string etiket, Control kontrol)
        {
            int satir = form.RowCount++;
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            form.Controls.Add(string.IsNullOrEmpty(etiket) ? new Label() : FormIskeleti.Etiket(etiket), 0, satir);
            form.Controls.Add(kontrol, 1, satir);
        }

        private void Yukle()
        {
            try
            {
                _dgv.DataSource = Veritabani.TabloGetir(
                    @"SELECT s.SakinID, s.Ad, s.Soyad, s.Telefon,
                             CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                             k.KullaniciAdi, s.Aktif, s.KayitTarihi
                      FROM Sakinler s
                      INNER JOIN Daireler d ON d.DaireID = s.DaireID
                      LEFT JOIN Kullanicilar k ON k.KullaniciID = s.KullaniciID
                      ORDER BY s.SakinID DESC");
                UiTema.KolonBasliklari(_dgv,
                    ("SakinID", "ID"),
                    ("KullaniciAdi", "Kullanıcı adı"),
                    ("KayitTarihi", "Kayıt"));

                _cmbDaire.Items.Clear();
                foreach (ListeKalemi kalem in Veritabani.DaireListesi())
                {
                    _cmbDaire.Items.Add(kalem);
                }

                if (_cmbDaire.Items.Count > 0)
                {
                    _cmbDaire.SelectedIndex = 0;
                }

                _cmbKullanici.Items.Clear();
                _cmbKullanici.Items.Add(new ListeKalemi(0, "(Bağlama)"));
                foreach (System.Data.DataRow satir in Veritabani.TabloGetir(
                    @"SELECT KullaniciID, CONCAT(Ad, ' ', Soyad, ' (', KullaniciAdi, ')') AS Etiket
                      FROM Kullanicilar
                      WHERE RolAdi = @rol
                      ORDER BY Ad, Soyad",
                    ("@rol", Veritabani.RolSakin)).Rows)
                {
                    _cmbKullanici.Items.Add(new ListeKalemi(Convert.ToInt32(satir["KullaniciID"]), Convert.ToString(satir["Etiket"]) ?? ""));
                }

                _cmbKullanici.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Sakin listesi yüklenemedi");
            }
        }

        private void BtnEkle_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtAd.Text) || string.IsNullOrWhiteSpace(_txtSoyad.Text))
            {
                FormIskeleti.Uyari("Eksik veya hatalı bilgi var. Ad ve soyad zorunludur.");
                return;
            }

            if (_cmbDaire.SelectedItem is not ListeKalemi daire)
            {
                FormIskeleti.Uyari("Seçilen daire bulunmuyor. Önce blok ve daire kaydı oluşturun.");
                return;
            }

            int kullaniciId = _cmbKullanici.SelectedItem is ListeKalemi k && k.Id > 0 ? k.Id : 0;

            try
            {
                if (Veritabani.Sayi("SELECT COUNT(*) FROM Daireler WHERE DaireID = @id", ("@id", daire.Id)) == 0)
                {
                    FormIskeleti.Uyari("Seçilen daire bulunmuyor, kayıt tamamlanmadı.");
                    return;
                }

                Veritabani.Calistir(
                    @"INSERT INTO Sakinler (DaireID, KullaniciID, Ad, Soyad, Telefon, Aktif)
                      VALUES (@daire, @kullanici, @ad, @soyad, @telefon, 1)",
                    ("@daire", daire.Id),
                    ("@kullanici", kullaniciId == 0 ? null : kullaniciId),
                    ("@ad", _txtAd.Text.Trim()),
                    ("@soyad", _txtSoyad.Text.Trim()),
                    ("@telefon", string.IsNullOrWhiteSpace(_txtTelefon.Text) ? null : _txtTelefon.Text.Trim()));

                if (kullaniciId > 0)
                {
                    Veritabani.Calistir("UPDATE Daireler SET KullaniciID = @k WHERE DaireID = @d", ("@k", kullaniciId), ("@d", daire.Id));
                    Veritabani.Calistir("UPDATE Kullanicilar SET Telefon = COALESCE(Telefon, @t) WHERE KullaniciID = @k",
                        ("@t", _txtTelefon.Text.Trim()), ("@k", kullaniciId));
                }

                _txtAd.Clear();
                _txtSoyad.Clear();
                _txtTelefon.Clear();
                FormIskeleti.Bilgi("Sakin kaydı oluşturuldu.");
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Sakin kaydı oluşturulamadı");
            }
        }
    }
}
