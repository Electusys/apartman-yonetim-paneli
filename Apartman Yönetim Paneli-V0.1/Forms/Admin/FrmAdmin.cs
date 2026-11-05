using MySqlConnector;

namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Yalnızca Admin rolüyle açılan ekran.
    /// Buradan yeni Apartman Yöneticisi hesabı oluşturulur ve mevcut yöneticiler listelenir.
    /// </summary>
    public partial class FrmAdmin : Form
    {
        private readonly string _adminAdSoyad;

        public FrmAdmin() : this("Admin")
        {
        }

        public FrmAdmin(string adminAdSoyad)
        {
            _adminAdSoyad = adminAdSoyad;
            InitializeComponent();
        }

        private void FrmAdmin_Load(object? sender, EventArgs e)
        {
            UiTema.FormAyarla(this);
            tableAna.BackColor = UiTema.ArkaPlan;
            pnlSag.BackColor = UiTema.ArkaPlan;
            pnlKart.BackColor = UiTema.Kart;
            UiTema.Yuvarla(pnlKart, 16);
            lblBaslik.ForeColor = UiTema.Yazi;
            lblAltBaslik.ForeColor = UiTema.YaziSoluk;
            lblListeBaslik.ForeColor = UiTema.Yazi;
            lblAd.ForeColor = UiTema.YaziSoluk;
            lblSoyad.ForeColor = UiTema.YaziSoluk;
            lblKullaniciAdi.ForeColor = UiTema.YaziSoluk;
            lblSifre.ForeColor = UiTema.YaziSoluk;
            lblSifreTekrar.ForeColor = UiTema.YaziSoluk;
            UiTema.MetinKutusu(txtAd);
            UiTema.MetinKutusu(txtSoyad);
            UiTema.MetinKutusu(txtKullaniciAdi);
            UiTema.MetinKutusu(txtSifre);
            UiTema.MetinKutusu(txtSifreTekrar);
            UiTema.BirincilButon(btnYoneticiOlustur);
            UiTema.IkincilButon(btnCikis);
            UiTema.Tablo(dgvYoneticiler);

            Label kullanici = new Label
            {
                Text = _adminAdSoyad,
                Height = 36,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = UiTema.Vurgu,
                TextAlign = ContentAlignment.MiddleLeft
            };
            Button cikis = btnCikis;
            UiTema.SolSeritKur(
                pnlSol,
                "Admin Paneli",
                "Yönetici rolünü yalnızca Admin atar. Muhasebeci ve teknisyen ataması yönetici panelinden yapılır.",
                kullanici,
                cikis);

            YoneticileriYukle();
        }

        private void btnYoneticiOlustur_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAd.Text) ||
                string.IsNullOrWhiteSpace(txtSoyad.Text) ||
                string.IsNullOrWhiteSpace(txtKullaniciAdi.Text) ||
                string.IsNullOrWhiteSpace(txtSifre.Text) ||
                string.IsNullOrWhiteSpace(txtSifreTekrar.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtSifre.Text != txtSifreTekrar.Text)
            {
                MessageBox.Show("Şifreler birbiriyle eşleşmiyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string ad = txtAd.Text.Trim();
            string soyad = txtSoyad.Text.Trim();
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Text;

            if (string.Equals(kullaniciAdi, Veritabani.AdminKullaniciAdi, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Admin kullanıcı adı değiştirilemez ve başkasına verilemez.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    ekleKomut.Parameters.AddWithValue("@rolAdi", Veritabani.RolYonetici);
                    ekleKomut.ExecuteNonQuery();
                }

                MessageBox.Show("Apartman yöneticisi hesabı oluşturuldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAd.Clear();
                txtSoyad.Clear();
                txtKullaniciAdi.Clear();
                txtSifre.Clear();
                txtSifreTekrar.Clear();
                YoneticileriYukle();
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                MessageBox.Show("Bu kullanıcı adı zaten kayıtlı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yönetici oluşturulurken bir hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void YoneticileriYukle()
        {
            try
            {
                using MySqlConnection baglanti = new MySqlConnection(Veritabani.ConnectionString);
                baglanti.Open();
                using MySqlCommand komut = new MySqlCommand(
                    @"SELECT KullaniciID, Ad, Soyad, KullaniciAdi, RolAdi
                      FROM Kullanicilar
                      WHERE RolAdi = @rolAdi
                      ORDER BY KullaniciID DESC", baglanti);
                komut.Parameters.AddWithValue("@rolAdi", Veritabani.RolYonetici);

                using MySqlDataAdapter adaptor = new MySqlDataAdapter(komut);
                System.Data.DataTable tablo = new System.Data.DataTable();
                adaptor.Fill(tablo);
                dgvYoneticiler.DataSource = tablo;

                if (dgvYoneticiler.Columns["KullaniciID"] is DataGridViewColumn idKolon)
                {
                    idKolon.HeaderText = "ID";
                }
                if (dgvYoneticiler.Columns["KullaniciAdi"] is DataGridViewColumn kullaniciKolon)
                {
                    kullaniciKolon.HeaderText = "Kullanıcı Adı";
                }
                if (dgvYoneticiler.Columns["RolAdi"] is DataGridViewColumn rolKolon)
                {
                    rolKolon.HeaderText = "Rol";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yönetici listesi yüklenemedi:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCikis_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
