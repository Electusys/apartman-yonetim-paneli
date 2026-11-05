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
}
