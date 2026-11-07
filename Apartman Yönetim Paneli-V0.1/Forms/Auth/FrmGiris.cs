using MySqlConnector;

namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Uygulamanın ilk ekranı: kullanıcı adı/şifre ile giriş veya kayıt formuna geçiş.
    /// Başarılı girişte rol Admin ise FrmAdmin, değilse FrmAnaPanel (rol paneli) açılır.
    /// </summary>
    public partial class FrmGiris : Form
    {
        public FrmGiris()
        {
            InitializeComponent();
        }

        private void FrmGiris_Load(object? sender, EventArgs e)
        {
            UiTema.FormAyarla(this);
            tableAna.BackColor = UiTema.ArkaPlan;
            pnlSag.BackColor = UiTema.ArkaPlan;
            lblBaslik.ForeColor = UiTema.Yazi;
            lblAltBaslik.ForeColor = UiTema.YaziSoluk;
            lblKullaniciAdi.ForeColor = UiTema.YaziSoluk;
            lblSifre.ForeColor = UiTema.YaziSoluk;
            lblHesapYok.ForeColor = UiTema.YaziSoluk;
            UiTema.MetinKutusu(txtKullaniciAdi);
            UiTema.MetinKutusu(txtSifre);
            UiTema.BirincilButon(btnGirisYap);
            UiTema.IkincilButon(btnKayitOl);

            UiTema.SolSeritKur(
                pnlSol,
                "Yönetim Paneli",
                "Sakin kayıtları açık. Yönetici hesapları yalnızca sistem yöneticisi tarafından oluşturulur.");

            try
            {
                Veritabani.AdminHesabiniHazirla();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Admin hesabı hazırlanırken bir hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGirisYap_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKullaniciAdi.Text) || string.IsNullOrWhiteSpace(txtSifre.Text))
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş bırakılamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Text;

            try
            {
                using MySqlConnection baglanti = new MySqlConnection(Veritabani.ConnectionString);
                baglanti.Open();
                using MySqlCommand komut = new MySqlCommand(
                    @"SELECT KullaniciID, Ad, Soyad, RolAdi, Durum
                      FROM Kullanicilar
                      WHERE KullaniciAdi = @kullaniciAdi AND Sifre = @sifre
                      LIMIT 1", baglanti);
                komut.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                komut.Parameters.AddWithValue("@sifre", sifre);

                using MySqlDataReader okuyucu = komut.ExecuteReader();
                if (!okuyucu.Read())
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int kullaniciId = okuyucu.GetInt32("KullaniciID");
                string adSoyad = $"{okuyucu.GetString("Ad")} {okuyucu.GetString("Soyad")}";
                string rolAdi = okuyucu.GetString("RolAdi");
                string durum = okuyucu.IsDBNull(okuyucu.GetOrdinal("Durum"))
                    ? Veritabani.DurumAktif
                    : okuyucu.GetString("Durum");

                if (string.Equals(durum, Veritabani.DurumPasif, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Kullanıcı hesabı pasif. Sisteme girişe izin verilmez.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Hide();
                txtSifre.Clear();

                Form sonrakiForm = string.Equals(rolAdi, Veritabani.RolAdmin, StringComparison.OrdinalIgnoreCase)
                    ? new FrmAdmin(adSoyad)
                    : new FrmAnaPanel(kullaniciId, adSoyad, rolAdi);

                sonrakiForm.FormClosed += (_, _) => Show();
                sonrakiForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Giriş sırasında bir hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnKayitOl_Click(object? sender, EventArgs e)
        {
            Hide();
            using FrmKayit kayitFormu = new FrmKayit();
            kayitFormu.ShowDialog();
            Show();
        }
    }
}
