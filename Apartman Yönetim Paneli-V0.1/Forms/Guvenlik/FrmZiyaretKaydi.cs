namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Misafir, kurye ve araç giriş/çıkış kaydı. Yalnızca bu alanlar istenir.
    /// </summary>
    public class FrmZiyaretKaydi : Form
    {
        private readonly int _kullaniciId;
        private readonly ComboBox _cmbTur;
        private readonly TextBox _txtAdSoyad;
        private readonly ComboBox _cmbDaire;
        private readonly DateTimePicker _dtpGiris;
        private readonly TextBox _txtPlaka;
        private readonly DataGridView _dgv;

        public FrmZiyaretKaydi(int kullaniciId, string adSoyad)
        {
            _kullaniciId = kullaniciId;
            Panel govde = FormIskeleti.Olustur(
                this,
                adSoyad,
                "Ziyaret Girişi",
                "Misafir, kurye veya araç girişini kaydedin. Çıkış saati, kişi ayrılınca yazılır.");

            _cmbTur = FormIskeleti.Combo();
            _cmbTur.Items.AddRange(new object[] { "Misafir", "Kurye", "Araç" });
            _cmbTur.SelectedIndex = 0;
            _cmbTur.SelectedIndexChanged += (_, _) => PlakaDurumu();
            _txtAdSoyad = FormIskeleti.Metin();
            _txtAdSoyad.MaxLength = 100;
            _cmbDaire = FormIskeleti.Combo();
            _dtpGiris = FormIskeleti.Tarih("dd.MM.yyyy HH:mm");
            _dtpGiris.Value = DateTime.Now;
            _txtPlaka = FormIskeleti.Metin();
            _txtPlaka.MaxLength = 15;

            Button btnKaydet = FormIskeleti.Birincil("Girişi Kaydet");
            btnKaydet.Click += BtnKaydet_Click;
            Button btnCikis = new Button { Text = "Çıkış Saatini Yaz", Width = 220, Height = 44 };
            UiTema.IkincilButon(btnCikis);
            btnCikis.Click += BtnCikis_Click;
            FlowLayoutPanel dugmeler = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight
            };
            dugmeler.Controls.Add(btnKaydet);
            dugmeler.Controls.Add(btnCikis);

            TableLayoutPanel form = GuvenlikOrtak.Alan();
            GuvenlikOrtak.Satir(form, "Tür", _cmbTur);
            GuvenlikOrtak.Satir(form, "Ad soyad", _txtAdSoyad);
            GuvenlikOrtak.Satir(form, "Ziyaret edilen daire", _cmbDaire);
            GuvenlikOrtak.Satir(form, "Giriş zamanı", _dtpGiris);
            GuvenlikOrtak.Satir(form, "Plaka (yalnız araç)", _txtPlaka);
            GuvenlikOrtak.Satir(form, "", dugmeler);

            _dgv = FormIskeleti.Tablo();
            TableLayoutPanel tablo = GuvenlikOrtak.IkiBolum();
            tablo.Controls.Add(form, 0, 0);
            tablo.Controls.Add(_dgv, 0, 1);
            govde.Controls.Add(tablo);
            PlakaDurumu();
            Yukle();
        }

        private bool AracMi() => string.Equals(_cmbTur.SelectedItem?.ToString(), "Araç", StringComparison.Ordinal);

        private void PlakaDurumu()
        {
            bool arac = AracMi();
            _txtPlaka.Enabled = arac;
            if (!arac)
            {
                _txtPlaka.Clear();
            }
        }

        private void Yukle()
        {
            try
            {
                GuvenlikOrtak.DaireCombo(_cmbDaire, false);
                _dgv.DataSource = Veritabani.TabloGetir(
                    @"SELECT z.ZiyaretID, z.Tur, z.AdSoyad,
                             CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                             z.Plaka, z.GirisZamani, z.CikisZamani
                      FROM ZiyaretKayitlari z
                      INNER JOIN Daireler d ON d.DaireID = z.DaireID
                      ORDER BY z.ZiyaretID DESC");
                UiTema.KolonBasliklari(_dgv,
                    ("ZiyaretID", "ID"),
                    ("AdSoyad", "Ad soyad"),
                    ("GirisZamani", "Giriş"),
                    ("CikisZamani", "Çıkış"));
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Ziyaret listesi yüklenemedi");
            }
        }

        private void BtnKaydet_Click(object? sender, EventArgs e)
        {
            string adSoyad = _txtAdSoyad.Text.Trim();
            if (string.IsNullOrWhiteSpace(adSoyad))
            {
                FormIskeleti.Uyari("Ad soyad zorunludur.");
                return;
            }

            if (GuvenlikOrtak.DaireId(_cmbDaire) is not int daireId)
            {
                FormIskeleti.Uyari("Ziyaret edilen daireyi seçin. Daire yoksa önce yönetici daire kaydı açmalıdır.");
                return;
            }

            string tur = _cmbTur.SelectedItem?.ToString() ?? "Misafir";
            string? plaka = null;
            if (AracMi())
            {
                plaka = GuvenlikOrtak.Plaka(_txtPlaka.Text);
                if (!GuvenlikOrtak.PlakaGecerli(plaka))
                {
                    FormIskeleti.Uyari("Araç girişinde geçerli bir plaka zorunludur.");
                    return;
                }
            }

            try
            {
                Veritabani.Calistir(
                    @"INSERT INTO ZiyaretKayitlari
                        (Tur, AdSoyad, DaireID, GirisZamani, Plaka, KaydedenKullaniciID)
                      VALUES (@tur, @ad, @daire, @giris, @plaka, @kullanici)",
                    ("@tur", tur == "Araç" ? "Arac" : tur),
                    ("@ad", adSoyad),
                    ("@daire", daireId),
                    ("@giris", _dtpGiris.Value),
                    ("@plaka", plaka),
                    ("@kullanici", _kullaniciId));
                GuvenlikOrtak.Yaz(_kullaniciId, "Ziyaret girişi", $"{tur} girişi kaydedildi. {adSoyad}");
                _txtAdSoyad.Clear();
                _txtPlaka.Clear();
                FormIskeleti.Bilgi("Giriş kaydı oluşturuldu.");
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Giriş kaydı oluşturulamadı");
            }
        }

        private void BtnCikis_Click(object? sender, EventArgs e)
        {
            if (GuvenlikOrtak.SeciliId(_dgv, "ZiyaretID") is not int id)
            {
                FormIskeleti.Uyari("Çıkış yazmak için listeden bir kayıt seçin.");
                return;
            }

            try
            {
                int etkilenen = Veritabani.Calistir(
                    @"UPDATE ZiyaretKayitlari
                      SET CikisZamani = NOW()
                      WHERE ZiyaretID = @id AND CikisZamani IS NULL",
                    ("@id", id));
                if (etkilenen == 0)
                {
                    FormIskeleti.Uyari("Bu kaydın çıkış saati zaten yazılmış.");
                    return;
                }

                GuvenlikOrtak.Yaz(_kullaniciId, "Ziyaret çıkışı", $"Ziyaret {id} için çıkış saati yazıldı.");
                FormIskeleti.Bilgi("Çıkış saati kaydedildi.");
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Çıkış saati yazılamadı");
            }
        }
    }
}
