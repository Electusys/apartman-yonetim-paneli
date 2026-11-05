using MySqlConnector;

namespace Apartman_Yönetim_Paneli_V0._1
{

    /// <summary>
    /// Yönetici: dönem + tutar + son ödeme tarihi girer, daireleri seçer, borç satırları oluşur.
    /// Aynı daire + aynı dönem ikinci kez yazılmaz.
    /// </summary>
    public class FrmAidatYonetimi : Form
    {
        private readonly DateTimePicker _dtpDonem;
        private readonly NumericUpDown _nudTutar;
        private readonly DateTimePicker _dtpSonOdeme;
        private readonly CheckedListBox _lstDaireler;
        private readonly DataGridView _dgv;
        private readonly int _kullaniciId;

        public FrmAidatYonetimi(int kullaniciId, string adSoyad)
        {
            _kullaniciId = kullaniciId;
            Panel govde = FormIskeleti.Olustur(this, adSoyad, "Aidat Yönetimi", "Dönem ve tutarı girip aidat uygulanacak daireleri seçin.");

            TableLayoutPanel tablo = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
            tablo.RowStyles.Add(new RowStyle(SizeType.Absolute, 280F));
            tablo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            _dtpDonem = FormIskeleti.Tarih("yyyy-MM");
            _nudTutar = FormIskeleti.Para();
            _dtpSonOdeme = FormIskeleti.Tarih();
            _dtpSonOdeme.Value = DateTime.Today.AddDays(15);
            _lstDaireler = new CheckedListBox
            {
                Dock = DockStyle.Fill,
                BackColor = UiTema.Kart,
                ForeColor = UiTema.Yazi,
                BorderStyle = BorderStyle.FixedSingle,
                CheckOnClick = true
            };
            Button btnTumunu = new Button { Text = "Tüm daireleri seç", Width = 180, Height = 36 };
            UiTema.IkincilButon(btnTumunu);
            btnTumunu.Click += (_, _) =>
            {
                for (int i = 0; i < _lstDaireler.Items.Count; i++)
                {
                    _lstDaireler.SetItemChecked(i, true);
                }
            };
            Button btnOlustur = FormIskeleti.Birincil("Yeni Aidat Oluştur");
            btnOlustur.Click += BtnOlustur_Click;

            TableLayoutPanel form = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 6 };
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68F));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            form.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            form.Controls.Add(FormIskeleti.Etiket("Aidat dönemi"), 0, 0);
            form.Controls.Add(_dtpDonem, 1, 0);
            form.Controls.Add(FormIskeleti.Etiket("Tutar"), 0, 1);
            form.Controls.Add(_nudTutar, 1, 1);
            form.Controls.Add(FormIskeleti.Etiket("Son ödeme tarihi"), 0, 2);
            form.Controls.Add(_dtpSonOdeme, 1, 2);
            form.Controls.Add(FormIskeleti.Etiket("Uygulanacak daireler"), 0, 3);
            form.Controls.Add(_lstDaireler, 1, 3);
            form.Controls.Add(new Label(), 0, 4);
            form.Controls.Add(btnTumunu, 1, 4);
            form.Controls.Add(new Label(), 0, 5);
            form.Controls.Add(btnOlustur, 1, 5);

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
                    @"SELECT b.AidatBorcID, dnm.Donem, CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                             b.Tutar, b.KalanTutar, b.Durum, dnm.SonOdemeTarihi
                      FROM AidatBorclari b
                      INNER JOIN AidatDonemleri dnm ON dnm.AidatDonemID = b.AidatDonemID
                      INNER JOIN Daireler d ON d.DaireID = b.DaireID
                      ORDER BY dnm.Donem DESC, d.Blok, d.DaireNo");
                UiTema.KolonBasliklari(_dgv,
                    ("AidatBorcID", "ID"),
                    ("KalanTutar", "Kalan"),
                    ("SonOdemeTarihi", "Son ödeme"));

                _lstDaireler.Items.Clear();
                foreach (ListeKalemi kalem in Veritabani.DaireListesi())
                {
                    _lstDaireler.Items.Add(kalem, true);
                }
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Aidat listesi yüklenemedi");
            }
        }

        private void BtnOlustur_Click(object? sender, EventArgs e)
        {
            if (_nudTutar.Value <= 0)
            {
                FormIskeleti.Uyari("Tutar geçersiz. Sıfırdan büyük bir tutar giriniz.");
                return;
            }

            List<int> daireler = new List<int>();
            foreach (object item in _lstDaireler.CheckedItems)
            {
                if (item is ListeKalemi kalem)
                {
                    daireler.Add(kalem.Id);
                }
            }

            if (daireler.Count == 0)
            {
                FormIskeleti.Uyari("En az bir daire seçiniz.");
                return;
            }

            string donem = _dtpDonem.Value.ToString("yyyy-MM");
            decimal tutar = _nudTutar.Value;
            DateTime sonOdeme = _dtpSonOdeme.Value.Date;
            int eklenen = 0;
            int atlanan = 0;

            try
            {
                using MySqlConnection baglanti = Veritabani.BaglantiAc();
                using MySqlTransaction tx = baglanti.BeginTransaction();

                long donemId;
                using (MySqlCommand ara = new MySqlCommand("SELECT AidatDonemID FROM AidatDonemleri WHERE Donem = @donem", baglanti, tx))
                {
                    ara.Parameters.AddWithValue("@donem", donem);
                    object? id = ara.ExecuteScalar();
                    if (id == null)
                    {
                        using MySqlCommand ekle = new MySqlCommand(
                            @"INSERT INTO AidatDonemleri (Donem, Tutar, SonOdemeTarihi, OlusturanKullaniciID)
                              VALUES (@donem, @tutar, @son, @kullanici)", baglanti, tx);
                        ekle.Parameters.AddWithValue("@donem", donem);
                        ekle.Parameters.AddWithValue("@tutar", tutar);
                        ekle.Parameters.AddWithValue("@son", sonOdeme);
                        ekle.Parameters.AddWithValue("@kullanici", _kullaniciId);
                        ekle.ExecuteNonQuery();
                        donemId = ekle.LastInsertedId;
                    }
                    else
                    {
                        donemId = Convert.ToInt64(id);
                    }
                }

                foreach (int daireId in daireler)
                {
                    using MySqlCommand kontrol = new MySqlCommand(
                        "SELECT COUNT(*) FROM AidatBorclari WHERE AidatDonemID = @donem AND DaireID = @daire", baglanti, tx);
                    kontrol.Parameters.AddWithValue("@donem", donemId);
                    kontrol.Parameters.AddWithValue("@daire", daireId);
                    if (Convert.ToInt32(kontrol.ExecuteScalar()) > 0)
                    {
                        atlanan++;
                        continue;
                    }

                    using MySqlCommand borc = new MySqlCommand(
                        @"INSERT INTO AidatBorclari (AidatDonemID, DaireID, Tutar, KalanTutar, Durum)
                          VALUES (@donem, @daire, @tutar, @tutar, @durum)", baglanti, tx);
                    borc.Parameters.AddWithValue("@donem", donemId);
                    borc.Parameters.AddWithValue("@daire", daireId);
                    borc.Parameters.AddWithValue("@tutar", tutar);
                    borc.Parameters.AddWithValue("@durum", Veritabani.BorcOdenmedi);
                    borc.ExecuteNonQuery();
                    eklenen++;
                }

                tx.Commit();
                FormIskeleti.Bilgi($"{eklenen} daire için aidat borcu oluşturuldu." + (atlanan > 0 ? $" {atlanan} dairede bu dönem zaten kayıtlıydı, mükerrer yazılmadı." : ""));
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Aidat oluşturulamadı");
            }
        }
    }


    /// <summary>
    /// Yönetici ve muhasebeci: seçilen aidat borcuna ödeme kaydeder, kalan tutarı günceller.
    /// </summary>
    public class FrmOdemeYonetimi : Form
    {
        private readonly DataGridView _dgvBorclar;
        private readonly DataGridView _dgvOdemeler;
        private readonly Label _lblOzet;
        private readonly NumericUpDown _nudTutar;
        private readonly DateTimePicker _dtpTarih;
        private readonly ComboBox _cmbYontem;
        private readonly int _kullaniciId;

        public FrmOdemeYonetimi(int kullaniciId, string adSoyad)
        {
            _kullaniciId = kullaniciId;
            Panel govde = FormIskeleti.Olustur(this, adSoyad, "Ödeme Yönetimi", "Daire aidat borcunu seçip ödemeyi kaydedin.");

            TableLayoutPanel tablo = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 4, ColumnCount = 1 };
            tablo.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            tablo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tablo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tablo.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));

            _dgvBorclar = FormIskeleti.Tablo();
            _dgvBorclar.SelectionChanged += (_, _) => OzetiGuncelle();
            _lblOzet = new Label { Dock = DockStyle.Fill, Height = 28, ForeColor = UiTema.Vurgu, Text = "Bir borç satırı seçiniz." };
            _nudTutar = FormIskeleti.Para();
            _dtpTarih = FormIskeleti.Tarih();
            _cmbYontem = FormIskeleti.Combo();
            _cmbYontem.Items.AddRange(new object[] { "Nakit", "Havale", "EFT", "Kredi Kartı" });
            _cmbYontem.SelectedIndex = 0;
            Button btnKaydet = FormIskeleti.Birincil("Ödemeyi Kaydet");
            btnKaydet.Click += BtnKaydet_Click;

            TableLayoutPanel form = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 4 };
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            form.Controls.Add(FormIskeleti.Etiket("Ödeme tutarı"), 0, 0);
            form.Controls.Add(_nudTutar, 1, 0);
            form.Controls.Add(FormIskeleti.Etiket("Tarih"), 2, 0);
            form.Controls.Add(_dtpTarih, 3, 0);
            form.Controls.Add(FormIskeleti.Etiket("Yöntem"), 0, 1);
            form.Controls.Add(_cmbYontem, 1, 1);
            form.Controls.Add(btnKaydet, 3, 1);
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));

            _dgvOdemeler = FormIskeleti.Tablo();
            tablo.Controls.Add(_dgvBorclar, 0, 0);
            tablo.Controls.Add(_lblOzet, 0, 1);
            tablo.Controls.Add(form, 0, 2);
            tablo.Controls.Add(_dgvOdemeler, 0, 3);
            govde.Controls.Add(tablo);
            Yukle();
        }

        private int? SeciliBorcId()
        {
            if (_dgvBorclar.CurrentRow == null)
            {
                return null;
            }

            return Convert.ToInt32(_dgvBorclar.CurrentRow.Cells["AidatBorcID"].Value);
        }

        private void Yukle()
        {
            try
            {
                _dgvBorclar.DataSource = Veritabani.TabloGetir(
                    @"SELECT b.AidatBorcID, CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire, dnm.Donem,
                             b.Tutar, b.KalanTutar, b.Durum
                      FROM AidatBorclari b
                      INNER JOIN Daireler d ON d.DaireID = b.DaireID
                      INNER JOIN AidatDonemleri dnm ON dnm.AidatDonemID = b.AidatDonemID
                      ORDER BY b.KalanTutar DESC, dnm.Donem DESC");
                UiTema.KolonBasliklari(_dgvBorclar, ("AidatBorcID", "ID"), ("KalanTutar", "Kalan"));

                _dgvOdemeler.DataSource = Veritabani.TabloGetir(
                    @"SELECT o.OdemeID, CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire, dnm.Donem,
                             o.Tutar, o.OdemeTarihi, o.OdemeYontemi
                      FROM Odemeler o
                      INNER JOIN AidatBorclari b ON b.AidatBorcID = o.AidatBorcID
                      INNER JOIN Daireler d ON d.DaireID = b.DaireID
                      INNER JOIN AidatDonemleri dnm ON dnm.AidatDonemID = b.AidatDonemID
                      ORDER BY o.OdemeID DESC");
                UiTema.KolonBasliklari(_dgvOdemeler,
                    ("OdemeID", "ID"),
                    ("OdemeTarihi", "Tarih"),
                    ("OdemeYontemi", "Yöntem"));
                OzetiGuncelle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Ödeme listesi yüklenemedi");
            }
        }

        private void OzetiGuncelle()
        {
            if (_dgvBorclar.CurrentRow == null)
            {
                _lblOzet.Text = "Bir borç satırı seçiniz.";
                return;
            }

            decimal tutar = Convert.ToDecimal(_dgvBorclar.CurrentRow.Cells["Tutar"].Value);
            decimal kalan = Convert.ToDecimal(_dgvBorclar.CurrentRow.Cells["KalanTutar"].Value);
            string daire = Convert.ToString(_dgvBorclar.CurrentRow.Cells["Daire"].Value) ?? "";
            _lblOzet.Text = $"{daire} — Toplam borç: {tutar:N2} ₺  |  Kalan: {kalan:N2} ₺";
            if (kalan > 0)
            {
                _nudTutar.Value = Math.Min(_nudTutar.Maximum, kalan);
            }
        }

        private void BtnKaydet_Click(object? sender, EventArgs e)
        {
            int? borcId = SeciliBorcId();
            if (borcId == null)
            {
                FormIskeleti.Uyari("Önce bir daire / aidat kaydı seçiniz.");
                return;
            }

            if (_nudTutar.Value <= 0)
            {
                FormIskeleti.Uyari("Ödeme tutarı geçersiz.");
                return;
            }

            decimal kalan = Convert.ToDecimal(_dgvBorclar.CurrentRow!.Cells["KalanTutar"].Value);
            decimal odeme = _nudTutar.Value;
            if (odeme > kalan)
            {
                DialogResult cevap = MessageBox.Show(
                    $"Ödeme tutarı kalan borçtan ({kalan:N2} ₺) büyük. İşlem kalan tutarla sınırlansın mı?",
                    "Kontrol",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (cevap != DialogResult.Yes)
                {
                    return;
                }

                odeme = kalan;
            }

            if (odeme <= 0)
            {
                FormIskeleti.Uyari("Bu borcun kalan tutarı yok.");
                return;
            }

            try
            {
                Veritabani.Calistir(
                    @"INSERT INTO Odemeler (AidatBorcID, Tutar, OdemeTarihi, OdemeYontemi, KaydedenKullaniciID)
                      VALUES (@borc, @tutar, @tarih, @yontem, @kullanici)",
                    ("@borc", borcId.Value),
                    ("@tutar", odeme),
                    ("@tarih", _dtpTarih.Value.Date),
                    ("@yontem", _cmbYontem.SelectedItem?.ToString() ?? "Nakit"),
                    ("@kullanici", _kullaniciId));

                decimal yeniKalan = kalan - odeme;
                decimal toplam = Convert.ToDecimal(_dgvBorclar.CurrentRow.Cells["Tutar"].Value);
                Veritabani.Calistir(
                    "UPDATE AidatBorclari SET KalanTutar = @kalan, Durum = @durum WHERE AidatBorcID = @id",
                    ("@kalan", yeniKalan),
                    ("@durum", Veritabani.BorcDurumu(yeniKalan, toplam)),
                    ("@id", borcId.Value));

                FormIskeleti.Bilgi("Ödeme kaydedildi, kalan borç güncellendi.");
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Ödeme kaydedilemedi");
            }
        }
    }


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
