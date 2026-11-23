namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Sakin talep ve şikayetini yöneticiye bilet olarak iletir.
    /// </summary>
    public class FrmYonetimBileti : Form
    {
        private readonly int _kullaniciId;
        private readonly TextBox _txtKonu;
        private readonly TextBox _txtAciklama;
        private readonly TextBox _txtSakin;
        private readonly ComboBox _cmbDaire;
        private readonly DataGridView _dgv;

        public FrmYonetimBileti(int kullaniciId, string adSoyad)
        {
            _kullaniciId = kullaniciId;
            Panel govde = FormIskeleti.Olustur(
                this,
                adSoyad,
                "Yönetim Bileti",
                "Sakinlerden gelen talep veya şikayeti yöneticiye iletin. Kayıt, sizi ve saatini saklar.");

            _txtKonu = FormIskeleti.Metin();
            _txtKonu.MaxLength = 120;
            _txtAciklama = FormIskeleti.Metin();
            _txtAciklama.MaxLength = 500;
            _txtSakin = FormIskeleti.Metin();
            _txtSakin.MaxLength = 100;
            _cmbDaire = FormIskeleti.Combo();
            Button btnKaydet = FormIskeleti.Birincil("Bileti İlet");
            btnKaydet.Click += BtnKaydet_Click;

            TableLayoutPanel form = GuvenlikOrtak.Alan();
            GuvenlikOrtak.Satir(form, "Konu", _txtKonu);
            GuvenlikOrtak.Satir(form, "Açıklama", _txtAciklama);
            GuvenlikOrtak.Satir(form, "Sakin ad soyad", _txtSakin);
            GuvenlikOrtak.Satir(form, "Daire", _cmbDaire);
            GuvenlikOrtak.Satir(form, "", btnKaydet);

            _dgv = FormIskeleti.Tablo();
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
                GuvenlikOrtak.DaireCombo(_cmbDaire, true);
                _dgv.DataSource = Veritabani.TabloGetir(
                    @"SELECT b.BiletID, b.Konu, b.SakinAdSoyad,
                             CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                             b.OlusturmaTarihi
                      FROM YonetimBiletleri b
                      LEFT JOIN Daireler d ON d.DaireID = b.DaireID
                      WHERE b.OlusturanKullaniciID = @id
                      ORDER BY b.BiletID DESC",
                    ("@id", _kullaniciId));
                UiTema.KolonBasliklari(_dgv,
                    ("BiletID", "ID"),
                    ("SakinAdSoyad", "Sakin"),
                    ("OlusturmaTarihi", "Tarih ve saat"));
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Biletler yüklenemedi");
            }
        }

        private void BtnKaydet_Click(object? sender, EventArgs e)
        {
            string konu = _txtKonu.Text.Trim();
            string aciklama = _txtAciklama.Text.Trim();
            if (string.IsNullOrWhiteSpace(konu) || string.IsNullOrWhiteSpace(aciklama))
            {
                FormIskeleti.Uyari("Konu ve açıklama zorunludur.");
                return;
            }

            int? daireId = GuvenlikOrtak.DaireId(_cmbDaire);
            string? sakin = string.IsNullOrWhiteSpace(_txtSakin.Text) ? null : _txtSakin.Text.Trim();
            try
            {
                Veritabani.Calistir(
                    @"INSERT INTO YonetimBiletleri
                        (Konu, Aciklama, SakinAdSoyad, DaireID, OlusturanKullaniciID)
                      VALUES (@konu, @aciklama, @sakin, @daire, @kullanici)",
                    ("@konu", konu),
                    ("@aciklama", aciklama),
                    ("@sakin", sakin),
                    ("@daire", daireId),
                    ("@kullanici", _kullaniciId));
                GuvenlikOrtak.Yaz(_kullaniciId, "Yönetim bileti", "Yöneticiye bilet iletildi: " + konu);
                _txtKonu.Clear();
                _txtAciklama.Clear();
                _txtSakin.Clear();
                FormIskeleti.Bilgi("Bilet yöneticiye iletildi.");
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Bilet iletilemedi");
            }
        }
    }
}
