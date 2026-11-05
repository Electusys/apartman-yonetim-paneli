namespace Apartman_Yönetim_Paneli_V0._1
{

    /// <summary>
    /// Yönetici: blok ekler, ardından bloğa daire bağlar.
    /// Daireler tablosunda Blok metin olarak tutulur (mevcut şema).
    /// </summary>
    public class FrmBlokDaire : Form
    {
        private readonly TextBox _txtBlokAdi;
        private readonly NumericUpDown _nudKatSayisi;
        private readonly TextBox _txtBlokAciklama;
        private readonly ComboBox _cmbBlok;
        private readonly TextBox _txtDaireNo;
        private readonly NumericUpDown _nudKat;
        private readonly DataGridView _dgvBloklar;
        private readonly DataGridView _dgvDaireler;

        public FrmBlokDaire(string adSoyad)
        {
            Panel govde = FormIskeleti.Olustur(this, adSoyad, "Blok ve Daire Yönetimi", "Blok ekleyin, sonra o bloğa daire kaydı oluşturun.");

            TableLayoutPanel tablo = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6
            };
            tablo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tablo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tablo.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tablo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tablo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tablo.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));

            _txtBlokAdi = FormIskeleti.Metin();
            _txtBlokAdi.MaxLength = 10;
            _nudKatSayisi = new NumericUpDown { Dock = DockStyle.Fill, Maximum = 50, Minimum = 1, Value = 5 };
            _txtBlokAciklama = FormIskeleti.Metin();
            Button btnBlok = FormIskeleti.Birincil("Yeni Blok Ekle");
            btnBlok.Click += BtnBlok_Click;

            TableLayoutPanel formBlok = IkiSutun(
                FormIskeleti.Etiket("Blok adı (en fazla 10 karakter)"), _txtBlokAdi,
                FormIskeleti.Etiket("Kat sayısı"), _nudKatSayisi,
                FormIskeleti.Etiket("Açıklama"), _txtBlokAciklama,
                new Label(), btnBlok);

            _dgvBloklar = FormIskeleti.Tablo();
            _cmbBlok = FormIskeleti.Combo();
            _txtDaireNo = FormIskeleti.Metin();
            _txtDaireNo.MaxLength = 10;
            _nudKat = new NumericUpDown { Dock = DockStyle.Fill, Maximum = 50, Minimum = 0 };
            Button btnDaire = FormIskeleti.Birincil("Daire Kaydı Oluştur");
            btnDaire.Click += BtnDaire_Click;

            TableLayoutPanel formDaire = IkiSutun(
                FormIskeleti.Etiket("Blok"), _cmbBlok,
                FormIskeleti.Etiket("Daire no"), _txtDaireNo,
                FormIskeleti.Etiket("Kat"), _nudKat,
                new Label(), btnDaire);

            _dgvDaireler = FormIskeleti.Tablo();

            tablo.Controls.Add(FormIskeleti.Etiket("Yeni blok"), 0, 0);
            tablo.Controls.Add(formBlok, 0, 1);
            tablo.Controls.Add(_dgvBloklar, 0, 2);
            tablo.Controls.Add(FormIskeleti.Etiket("Yeni daire"), 0, 3);
            tablo.Controls.Add(formDaire, 0, 4);
            tablo.Controls.Add(_dgvDaireler, 0, 5);
            govde.Controls.Add(tablo);

            Yukle();
        }

        private static TableLayoutPanel IkiSutun(params Control[] kontroller)
        {
            TableLayoutPanel p = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
            p.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            p.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            for (int i = 0; i < kontroller.Length; i += 2)
            {
                p.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                p.Controls.Add(kontroller[i], 0, i / 2);
                p.Controls.Add(kontroller[i + 1], 1, i / 2);
            }

            return p;
        }

        private void Yukle()
        {
            try
            {
                _dgvBloklar.DataSource = Veritabani.TabloGetir(
                    "SELECT BlokID, BlokAdi, KatSayisi, Aciklama, OlusturmaTarihi FROM Bloklar ORDER BY BlokID DESC");
                UiTema.KolonBasliklari(_dgvBloklar, ("BlokID", "ID"), ("BlokAdi", "Blok"), ("KatSayisi", "Kat sayısı"), ("Aciklama", "Açıklama"), ("OlusturmaTarihi", "Kayıt"));

                _dgvDaireler.DataSource = Veritabani.TabloGetir(
                    "SELECT DaireID, Blok, DaireNo, Kat, KullaniciID FROM Daireler ORDER BY Blok, DaireNo");
                UiTema.KolonBasliklari(_dgvDaireler, ("DaireID", "ID"), ("DaireNo", "Daire no"), ("KullaniciID", "Kullanıcı"));

                string? secili = _cmbBlok.SelectedItem?.ToString();
                _cmbBlok.Items.Clear();
                foreach (System.Data.DataRow satir in Veritabani.TabloGetir("SELECT BlokAdi FROM Bloklar ORDER BY BlokAdi").Rows)
                {
                    _cmbBlok.Items.Add(Convert.ToString(satir["BlokAdi"]) ?? "");
                }

                if (secili != null && _cmbBlok.Items.Contains(secili))
                {
                    _cmbBlok.SelectedItem = secili;
                }
                else if (_cmbBlok.Items.Count > 0)
                {
                    _cmbBlok.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Liste yüklenemedi");
            }
        }

        private void BtnBlok_Click(object? sender, EventArgs e)
        {
            string blokAdi = _txtBlokAdi.Text.Trim();
            if (string.IsNullOrWhiteSpace(blokAdi))
            {
                FormIskeleti.Uyari("Zorunlu bilgiler eksik. Blok adı giriniz.");
                return;
            }

            try
            {
                if (Veritabani.Sayi("SELECT COUNT(*) FROM Bloklar WHERE BlokAdi = @ad", ("@ad", blokAdi)) > 0)
                {
                    FormIskeleti.Uyari("Bu blok adı zaten kayıtlı.");
                    return;
                }

                Veritabani.Calistir(
                    "INSERT INTO Bloklar (BlokAdi, KatSayisi, Aciklama) VALUES (@ad, @kat, @aciklama)",
                    ("@ad", blokAdi),
                    ("@kat", (int)_nudKatSayisi.Value),
                    ("@aciklama", string.IsNullOrWhiteSpace(_txtBlokAciklama.Text) ? null : _txtBlokAciklama.Text.Trim()));
                _txtBlokAdi.Clear();
                _txtBlokAciklama.Clear();
                FormIskeleti.Bilgi("Blok kaydı oluşturuldu.");
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Blok eklenirken hata oluştu");
            }
        }

        private void BtnDaire_Click(object? sender, EventArgs e)
        {
            string? blok = _cmbBlok.SelectedItem?.ToString();
            string daireNo = _txtDaireNo.Text.Trim();
            if (string.IsNullOrWhiteSpace(blok) || string.IsNullOrWhiteSpace(daireNo))
            {
                FormIskeleti.Uyari("Zorunlu bilgiler eksik. Blok ve daire numarasını giriniz.");
                return;
            }

            try
            {
                if (Veritabani.Sayi("SELECT COUNT(*) FROM Daireler WHERE Blok = @blok AND DaireNo = @no", ("@blok", blok), ("@no", daireNo)) > 0)
                {
                    FormIskeleti.Uyari("Aynı blokta bu daire numarası zaten var. Mükerrer kayıt engellendi.");
                    return;
                }

                Veritabani.Calistir(
                    "INSERT INTO Daireler (Blok, DaireNo, Kat) VALUES (@blok, @no, @kat)",
                    ("@blok", blok),
                    ("@no", daireNo),
                    ("@kat", (int)_nudKat.Value));
                _txtDaireNo.Clear();
                FormIskeleti.Bilgi("Daire kaydı oluşturuldu.");
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Daire eklenirken hata oluştu");
            }
        }
    }


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
