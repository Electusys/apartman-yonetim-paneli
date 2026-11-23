namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Yönetici, plaka sorgusunda kullanılacak daire araçlarını kaydeder.
    /// </summary>
    public class FrmKayitliAraclar : Form
    {
        private readonly int _kullaniciId;
        private readonly TextBox _txtPlaka;
        private readonly ComboBox _cmbDaire;
        private readonly DataGridView _dgv;

        public FrmKayitliAraclar(int kullaniciId, string adSoyad)
        {
            _kullaniciId = kullaniciId;
            Panel govde = FormIskeleti.Olustur(
                this,
                adSoyad,
                "Kayıtlı Araçlar",
                "Daireye ait plakayı kaydedin. Güvenlik personeli bu listeden yalnızca daire ve sakin adını görür.");

            _txtPlaka = FormIskeleti.Metin();
            _txtPlaka.MaxLength = 15;
            _cmbDaire = FormIskeleti.Combo();
            Button btnKaydet = FormIskeleti.Birincil("Aracı Kaydet");
            btnKaydet.Click += BtnKaydet_Click;
            Button btnSil = new Button { Text = "Seçili kaydı sil", Width = 220, Height = 44 };
            UiTema.IkincilButon(btnSil);
            btnSil.Click += BtnSil_Click;
            FlowLayoutPanel dugmeler = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                WrapContents = false
            };
            dugmeler.Controls.Add(btnKaydet);
            dugmeler.Controls.Add(btnSil);

            TableLayoutPanel form = GuvenlikOrtak.Alan();
            GuvenlikOrtak.Satir(form, "Plaka", _txtPlaka);
            GuvenlikOrtak.Satir(form, "Daire", _cmbDaire);
            GuvenlikOrtak.Satir(form, "", dugmeler);

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
                    @"SELECT a.AracID, a.Plaka,
                             CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                             a.KayitTarihi
                      FROM KayitliAraclar a
                      INNER JOIN Daireler d ON d.DaireID = a.DaireID
                      ORDER BY a.Plaka");
                UiTema.KolonBasliklari(_dgv, ("AracID", "ID"), ("KayitTarihi", "Kayıt"));
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Araç listesi yüklenemedi");
            }
        }

        private void BtnKaydet_Click(object? sender, EventArgs e)
        {
            string plaka = GuvenlikOrtak.Plaka(_txtPlaka.Text);
            if (!GuvenlikOrtak.PlakaGecerli(plaka))
            {
                FormIskeleti.Uyari("Geçerli bir plaka girin.");
                return;
            }

            if (GuvenlikOrtak.DaireId(_cmbDaire) is not int daireId)
            {
                FormIskeleti.Uyari("Aracın bağlı olduğu daireyi seçin.");
                return;
            }

            try
            {
                Veritabani.Calistir(
                    @"INSERT INTO KayitliAraclar (DaireID, Plaka, KaydedenKullaniciID)
                      VALUES (@daire, @plaka, @kullanici)",
                    ("@daire", daireId),
                    ("@plaka", plaka),
                    ("@kullanici", _kullaniciId == 0 ? null : _kullaniciId));
                _txtPlaka.Clear();
                FormIskeleti.Bilgi("Araç kaydı oluşturuldu.");
                Yukle();
            }
            catch (MySqlConnector.MySqlException ex) when (ex.Number == 1062)
            {
                FormIskeleti.Uyari("Bu plaka zaten kayıtlı.");
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Araç kaydı oluşturulamadı");
            }
        }

        private void BtnSil_Click(object? sender, EventArgs e)
        {
            if (GuvenlikOrtak.SeciliId(_dgv, "AracID") is not int id)
            {
                FormIskeleti.Uyari("Silmek için listeden bir araç seçin.");
                return;
            }

            if (MessageBox.Show("Seçili araç kaydı silinsin mi?", "Araç", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                Veritabani.Calistir("DELETE FROM KayitliAraclar WHERE AracID = @id", ("@id", id));
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Araç kaydı silinemedi");
            }
        }
    }
}