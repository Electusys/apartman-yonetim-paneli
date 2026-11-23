namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Plaka sorgusu ve misafir araçlar için geçici park izni.
    /// </summary>
    public class FrmPlakaPark : Form
    {
        private readonly int _kullaniciId;
        private readonly TextBox _txtSorgu;
        private readonly Label _lblSonuc;
        private readonly TextBox _txtPlaka;
        private readonly TextBox _txtAdSoyad;
        private readonly ComboBox _cmbDaire;
        private readonly DateTimePicker _dtpBaslangic;
        private readonly DateTimePicker _dtpBitis;
        private readonly DataGridView _dgv;

        public FrmPlakaPark(int kullaniciId, string adSoyad)
        {
            _kullaniciId = kullaniciId;
            Panel govde = FormIskeleti.Olustur(
                this,
                adSoyad,
                "Plaka ve Park",
                "Plakanın kayıtlı olup olmadığını ve bağlı daireyi görün. Misafir araca başlangıç ve bitiş saatli izin verin.");

            _txtSorgu = FormIskeleti.Metin();
            _txtSorgu.MaxLength = 15;
            Button btnSorgu = FormIskeleti.Birincil("Plaka Sorgula");
            btnSorgu.Click += BtnSorgu_Click;
            _lblSonuc = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = UiTema.Vurgu,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Text = "Sorgu sonucu burada görünür."
            };

            _txtPlaka = FormIskeleti.Metin();
            _txtPlaka.MaxLength = 15;
            _txtAdSoyad = FormIskeleti.Metin();
            _txtAdSoyad.MaxLength = 100;
            _cmbDaire = FormIskeleti.Combo();
            _dtpBaslangic = FormIskeleti.Tarih("dd.MM.yyyy HH:mm");
            _dtpBaslangic.Value = DateTime.Now;
            _dtpBitis = FormIskeleti.Tarih("dd.MM.yyyy HH:mm");
            _dtpBitis.Value = DateTime.Now.AddHours(2);
            Button btnIzin = FormIskeleti.Birincil("Geçici izin oluştur");
            btnIzin.Click += BtnIzin_Click;

            TableLayoutPanel form = GuvenlikOrtak.Alan();
            GuvenlikOrtak.Satir(form, "Sorgulanacak plaka", _txtSorgu);
            GuvenlikOrtak.Satir(form, "", btnSorgu);
            GuvenlikOrtak.Satir(form, "Sonuç", _lblSonuc);
            GuvenlikOrtak.Satir(form, "İzin plakası", _txtPlaka);
            GuvenlikOrtak.Satir(form, "Ziyaretçi ad soyad", _txtAdSoyad);
            GuvenlikOrtak.Satir(form, "İlgili daire", _cmbDaire);
            GuvenlikOrtak.Satir(form, "Başlangıç", _dtpBaslangic);
            GuvenlikOrtak.Satir(form, "Bitiş", _dtpBitis);
            GuvenlikOrtak.Satir(form, "", btnIzin);

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
                GuvenlikOrtak.DaireCombo(_cmbDaire, false);
                _dgv.DataSource = Veritabani.TabloGetir(
                    @"SELECT p.IzinID, p.Plaka, p.ZiyaretciAdSoyad,
                             CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                             p.BaslangicZamani, p.BitisZamani
                      FROM GeciciParkIzinleri p
                      INNER JOIN Daireler d ON d.DaireID = p.DaireID
                      ORDER BY p.IzinID DESC");
                UiTema.KolonBasliklari(_dgv,
                    ("IzinID", "ID"),
                    ("ZiyaretciAdSoyad", "Ziyaretçi"),
                    ("BaslangicZamani", "Başlangıç"),
                    ("BitisZamani", "Bitiş"));
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Park izinleri yüklenemedi");
            }
        }

        private void BtnSorgu_Click(object? sender, EventArgs e)
        {
            string plaka = GuvenlikOrtak.Plaka(_txtSorgu.Text);
            if (!GuvenlikOrtak.PlakaGecerli(plaka))
            {
                FormIskeleti.Uyari("Geçerli bir plaka girin.");
                return;
            }

            try
            {
                System.Data.DataTable kayit = Veritabani.TabloGetir(
                    @"SELECT Daire, Sakinler
                      FROM vw_GuvenlikPlaka
                      WHERE Plaka = @plaka",
                    ("@plaka", plaka));
                System.Data.DataTable izin = Veritabani.TabloGetir(
                    @"SELECT CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                             p.BaslangicZamani, p.BitisZamani
                      FROM GeciciParkIzinleri p
                      INNER JOIN Daireler d ON d.DaireID = p.DaireID
                      WHERE p.Plaka = @plaka
                        AND p.BaslangicZamani <= NOW()
                        AND p.BitisZamani >= NOW()
                      ORDER BY p.BitisZamani DESC
                      LIMIT 1",
                    ("@plaka", plaka));

                string sonuc;
                if (kayit.Rows.Count == 0)
                {
                    sonuc = $"{plaka} kayıtlı araçlarda yok.";
                }
                else
                {
                    string daire = Convert.ToString(kayit.Rows[0]["Daire"]) ?? "-";
                    string sakin = Convert.ToString(kayit.Rows[0]["Sakinler"]) ?? "";
                    sonuc = $"{plaka} kayıtlı. Daire: {daire}.";
                    if (!string.IsNullOrWhiteSpace(sakin))
                    {
                        sonuc += " Sakin: " + sakin + ".";
                    }
                }

                if (izin.Rows.Count > 0)
                {
                    sonuc += $" Aktif geçici izin: {izin.Rows[0]["Daire"]}, {izin.Rows[0]["BaslangicZamani"]:dd.MM.yyyy HH:mm} - {izin.Rows[0]["BitisZamani"]:dd.MM.yyyy HH:mm}.";
                }

                _lblSonuc.Text = sonuc;
                GuvenlikOrtak.Yaz(_kullaniciId, "Plaka sorgusu", sonuc);
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Plaka sorgulanamadı");
            }
        }

        private void BtnIzin_Click(object? sender, EventArgs e)
        {
            string plaka = GuvenlikOrtak.Plaka(_txtPlaka.Text);
            string ad = _txtAdSoyad.Text.Trim();
            if (!GuvenlikOrtak.PlakaGecerli(plaka) || string.IsNullOrWhiteSpace(ad))
            {
                FormIskeleti.Uyari("Plaka ve ziyaretçi ad soyad zorunludur.");
                return;
            }

            if (GuvenlikOrtak.DaireId(_cmbDaire) is not int daireId)
            {
                FormIskeleti.Uyari("İznin bağlı olduğu daireyi seçin.");
                return;
            }

            if (_dtpBitis.Value <= _dtpBaslangic.Value)
            {
                FormIskeleti.Uyari("Bitiş zamanı başlangıçtan sonra olmalıdır.");
                return;
            }

            try
            {
                Veritabani.Calistir(
                    @"INSERT INTO GeciciParkIzinleri
                        (Plaka, DaireID, ZiyaretciAdSoyad, BaslangicZamani, BitisZamani, KaydedenKullaniciID)
                      VALUES (@plaka, @daire, @ad, @bas, @bit, @kullanici)",
                    ("@plaka", plaka),
                    ("@daire", daireId),
                    ("@ad", ad),
                    ("@bas", _dtpBaslangic.Value),
                    ("@bit", _dtpBitis.Value),
                    ("@kullanici", _kullaniciId));
                GuvenlikOrtak.Yaz(
                    _kullaniciId,
                    "Park izni",
                    $"{plaka} için geçici izin: {_dtpBaslangic.Value:dd.MM.yyyy HH:mm} - {_dtpBitis.Value:dd.MM.yyyy HH:mm}");
                _txtPlaka.Clear();
                _txtAdSoyad.Clear();
                FormIskeleti.Bilgi("Geçici park izni oluşturuldu.");
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Park izni oluşturulamadı");
            }
        }
    }
}
