using MySqlConnector;

namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Herkese açık kayıt ekranı. Buradan yalnızca "Apartman Sakini" hesabı açılır.
    /// </summary>
    public partial class FrmKayit : Form
    {
        public FrmKayit()
        {
            InitializeComponent();
            Load += FrmKayit_Load;
        }

        private void FrmKayit_Load(object? sender, EventArgs e)
        {
            UiTema.FormAyarla(this);
            tableAna.BackColor = UiTema.ArkaPlan;
            pnlSag.BackColor = UiTema.ArkaPlan;
            lblBaslik.ForeColor = UiTema.Yazi;
            lblAltBaslik.ForeColor = UiTema.YaziSoluk;
            lblAd.ForeColor = UiTema.YaziSoluk;
            lblSoyad.ForeColor = UiTema.YaziSoluk;
            lblKullaniciAdi.ForeColor = UiTema.YaziSoluk;
            lblSifre.ForeColor = UiTema.YaziSoluk;
            lblSifreTekrar.ForeColor = UiTema.YaziSoluk;
            lblRolBilgi.ForeColor = UiTema.Vurgu;
            UiTema.MetinKutusu(txtAd);
            UiTema.MetinKutusu(txtSoyad);
            UiTema.MetinKutusu(txtKullaniciAdi);
            UiTema.MetinKutusu(txtSifre);
            UiTema.MetinKutusu(txtSifreTekrar);
            UiTema.BirincilButon(btnKayitOl);
            UiTema.IkincilButon(btnGirisDon);

            UiTema.SolSeritKur(
                pnlSol,
                "Kayıt Ol",
                "Bu ekrandan yalnızca apartman sakini hesabı oluşturulur. Yönetici hesapları Admin panelinden açılır.");
        }

        private void btnKayitOl_Click(object? sender, EventArgs e)
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
                MessageBox.Show("Bu kullanıcı adı sistem tarafından ayrılmıştır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        MessageBox.Show("Bu kullanıcı adı zaten kayıtlı. Lütfen başka bir kullanıcı adı seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    ekleKomut.Parameters.AddWithValue("@rolAdi", Veritabani.RolSakin);
                    ekleKomut.ExecuteNonQuery();
                }

                MessageBox.Show("Sakin hesabınız oluşturuldu. Giriş ekranından oturum açabilirsiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                MessageBox.Show("Bu kullanıcı adı zaten kayıtlı. Lütfen başka bir kullanıcı adı seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt sırasında bir hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGirisDon_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}