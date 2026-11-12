using MySqlConnector;

namespace Apartman_Yönetim_Paneli_V0._1
{

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
            Panel govde = FormIskeleti.Olustur(this, adSoyad, "Ödeme Yönetimi", "Blok ve daireyi seçip ödemeyi kaydedin.");

            TableLayoutPanel tablo = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 4, ColumnCount = 1 };
            tablo.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            tablo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tablo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tablo.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));

            _dgvBorclar = FormIskeleti.Tablo();
            _dgvBorclar.SelectionChanged += (_, _) => OzetiGuncelle();
            _lblOzet = new Label { Dock = DockStyle.Fill, Height = 28, ForeColor = UiTema.Vurgu, Text = "Blok / daire seçin." };
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
            Load += (_, _) => Yukle();
        }

        private int? SeciliBorcId()
        {
            if (_dgvBorclar.CurrentRow == null || !_dgvBorclar.Columns.Contains("AidatBorcID"))
            {
                return null;
            }

            object? id = _dgvBorclar.CurrentRow.Cells["AidatBorcID"].Value;
            if (id == null || id == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(id);
        }

        private void Yukle()
        {
            try
            {
                _dgvBorclar.DataSource = Veritabani.TabloGetir(
                    @"SELECT CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                             IFNULL(dnm.Donem, '—') AS Donem,
                             b.AidatBorcID, IFNULL(b.Tutar, 0) AS Tutar,
                             IFNULL(b.KalanTutar, 0) AS KalanTutar,
                             IFNULL(b.Durum, 'Borç yok') AS Durum
                      FROM Daireler d
                      LEFT JOIN AidatBorclari b ON b.DaireID = d.DaireID
                      LEFT JOIN AidatDonemleri dnm ON dnm.AidatDonemID = b.AidatDonemID
                      ORDER BY d.Blok, d.DaireNo, dnm.Donem DESC");
                UiTema.KolonBasliklari(_dgvBorclar, ("AidatBorcID", "Borç no"), ("KalanTutar", "Kalan"));

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
            if (_dgvBorclar.CurrentRow == null || !_dgvBorclar.Columns.Contains("Daire"))
            {
                _lblOzet.Text = "Blok / daire seçin.";
                return;
            }

            string daire = Convert.ToString(_dgvBorclar.CurrentRow.Cells["Daire"].Value) ?? "";
            if (SeciliBorcId() == null)
            {
                _lblOzet.Text = string.IsNullOrWhiteSpace(daire)
                    ? "Blok / daire seçin."
                    : $"{daire} — bu daire için aidat borcu yok.";
                return;
            }

            decimal tutar = Convert.ToDecimal(_dgvBorclar.CurrentRow.Cells["Tutar"].Value);
            decimal kalan = Convert.ToDecimal(_dgvBorclar.CurrentRow.Cells["KalanTutar"].Value);
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
                FormIskeleti.Uyari("Seçilen daire için açık aidat borcu yok. Önce yönetici aidat oluşturmalı.");
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
}
