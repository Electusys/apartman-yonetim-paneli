using MySqlConnector;

namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Apartman yöneticisinin muhasebeci ve teknisyen hesabı açtığı ekran.
    /// ComboBox'ta yalnızca bu iki rol vardır; yönetici kendine Admin veya başka yönetici veremez.
    /// </summary>
    public partial class FrmPersonelAtama : Form
    {
        private readonly string _yoneticiAdSoyad;

        public FrmPersonelAtama() : this("Yönetici")
        {
        }

        public FrmPersonelAtama(string yoneticiAdSoyad)
        {
            _yoneticiAdSoyad = yoneticiAdSoyad;
            InitializeComponent();
            UiTema.FormAyarla(this);
            tableAna.BackColor = UiTema.ArkaPlan;
            pnlSag.BackColor = UiTema.ArkaPlan;
            pnlKart.BackColor = UiTema.Kart;
            UiTema.Yuvarla(pnlKart, 16);
            lblBaslik.ForeColor = UiTema.Yazi;
            lblAltBaslik.ForeColor = UiTema.YaziSoluk;
            UiTema.MetinKutusu(txtAd);
            UiTema.MetinKutusu(txtSoyad);
            UiTema.MetinKutusu(txtKullaniciAdi);
            UiTema.MetinKutusu(txtSifre);
            UiTema.MetinKutusu(txtSifreTekrar);
            UiTema.Combo(cmbRol);
            UiTema.BirincilButon(btnPersonelOlustur);
            UiTema.IkincilButon(btnGeri);

            Label kullanici = new Label
            {
                Text = _yoneticiAdSoyad,
                Height = 36,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = UiTema.Vurgu,
                TextAlign = ContentAlignment.MiddleLeft
            };
            UiTema.SolSeritKur(
                pnlSol,
                "Personel Atama",
                "Yalnızca muhasebeci veya teknisyen hesabı oluşturabilirsiniz.",
                kullanici,
                btnGeri);

            cmbRol.Items.Clear();
            cmbRol.Items.Add(Veritabani.RolMuhasebeci);
            cmbRol.Items.Add(Veritabani.RolTeknisyen);
            cmbRol.SelectedIndex = 0;

            UiTema.Tablo(dgvPersonel);
            PersonelleriYukle();
        }

        private void FrmPersonelAtama_Load(object? sender, EventArgs e)
        {
        }

        private void btnPersonelOlustur_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAd.Text) ||
                string.IsNullOrWhiteSpace(txtSoyad.Text) ||
                string.IsNullOrWhiteSpace(txtKullaniciAdi.Text) ||
                string.IsNullOrWhiteSpace(txtSifre.Text) ||
                string.IsNullOrWhiteSpace(txtSifreTekrar.Text) ||
                cmbRol.SelectedIndex < 0)
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtSifre.Text != txtSifreTekrar.Text)
            {
                MessageBox.Show("Şifreler birbiriyle eşleşmiyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rolAdi = cmbRol.SelectedItem?.ToString() ?? string.Empty;
            if (rolAdi != Veritabani.RolMuhasebeci && rolAdi != Veritabani.RolTeknisyen)
            {
                MessageBox.Show("Yönetici yalnızca muhasebeci veya teknisyen atayabilir.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string ad = txtAd.Text.Trim();
            string soyad = txtSoyad.Text.Trim();
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Text;

            if (string.Equals(kullaniciAdi, Veritabani.AdminKullaniciAdi, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Admin kullanıcı adı başkasına verilemez.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using MySqlConnection baglanti = new MySqlConnection(Veritabani.ConnectionString);
                baglanti.Open();

                using (MySqlCommand kontrolKomut = new MySqlCommand(
                    "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi = @kullaniciAdi", baglanti))
                {
                    kontrolKomut.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                    int kayitSayisi = Convert.ToInt32(kontrolKomut.ExecuteScalar());
                    if (kayitSayisi > 0)
                    {
                        MessageBox.Show("Bu kullanıcı adı zaten kayıtlı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                using (MySqlCommand ekleKomut = new MySqlCommand(
                    @"INSERT INTO Kullanicilar (Ad, Soyad, KullaniciAdi, Sifre, RolAdi)
                      VALUES (@ad, @soyad, @kullaniciAdi, @sifre, @rolAdi)", baglanti))
                {
                    ekleKomut.Parameters.AddWithValue("@ad", ad);
                    ekleKomut.Parameters.AddWithValue("@soyad", soyad);
                    ekleKomut.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                    ekleKomut.Parameters.AddWithValue("@sifre", sifre);
                    ekleKomut.Parameters.AddWithValue("@rolAdi", rolAdi);
                    ekleKomut.ExecuteNonQuery();
                }

                MessageBox.Show($"{rolAdi} hesabı oluşturuldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAd.Clear();
                txtSoyad.Clear();
                txtKullaniciAdi.Clear();
                txtSifre.Clear();
                txtSifreTekrar.Clear();
                cmbRol.SelectedIndex = 0;
                PersonelleriYukle();
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                MessageBox.Show("Bu kullanıcı adı zaten kayıtlı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Personel oluşturulurken bir hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PersonelleriYukle()
        {
            try
            {
                using MySqlConnection baglanti = new MySqlConnection(Veritabani.ConnectionString);
                baglanti.Open();
                using MySqlCommand komut = new MySqlCommand(
                    @"SELECT KullaniciID, Ad, Soyad, KullaniciAdi, RolAdi
                      FROM Kullanicilar
                      WHERE RolAdi IN (@muhasebeci, @teknisyen)
                      ORDER BY RolAdi, KullaniciID DESC", baglanti);
                komut.Parameters.AddWithValue("@muhasebeci", Veritabani.RolMuhasebeci);
                komut.Parameters.AddWithValue("@teknisyen", Veritabani.RolTeknisyen);

                using MySqlDataAdapter adaptor = new MySqlDataAdapter(komut);
                System.Data.DataTable tablo = new System.Data.DataTable();
                adaptor.Fill(tablo);
                dgvPersonel.DataSource = tablo;

                if (dgvPersonel.Columns["KullaniciID"] is DataGridViewColumn idKolon)
                {
                    idKolon.HeaderText = "ID";
                }
                if (dgvPersonel.Columns["KullaniciAdi"] is DataGridViewColumn kullaniciKolon)
                {
                    kullaniciKolon.HeaderText = "Kullanıcı Adı";
                }
                if (dgvPersonel.Columns["RolAdi"] is DataGridViewColumn rolKolon)
                {
                    rolKolon.HeaderText = "Rol";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Personel listesi yüklenemedi:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGeri_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
