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

            Button btnKvkk = new Button
            {
                Text = "KVKK Aydınlatma Metni",
                Dock = DockStyle.Top,
                Height = 40
            };
            UiTema.IkincilButon(btnKvkk);
            btnKvkk.Click += (_, _) =>
            {
                using FrmKvkk kvkk = new FrmKvkk();
                kvkk.ShowDialog(this);
            };
            tableForm.Controls.Add(btnKvkk, 0, 9);

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
                GirisKaydi? giris = Hesaplar.GirisYap(kullaniciAdi, sifre);
                if (giris == null)
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int kullaniciId = giris.KullaniciId;
                string adSoyad = giris.AdSoyad;
                string rolAdi = giris.RolAdi;
                string durum = giris.Durum;

                if (string.Equals(durum, Veritabani.DurumPasif, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Kullanıcı hesabı pasif. Sisteme girişe izin verilmez.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.Equals(rolAdi, Veritabani.RolGuvenlik, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        GuvenlikOrtak.Yaz(kullaniciId, "Oturum açıldı", "Güvenlik oturumu açıldı.");
                    }
                    catch (Exception logEx)
                    {
                        MessageBox.Show(
                            "Oturum açıldı ancak işlem günlüğüne yazılamadı. Güvenlik tablolarını kurun.\n" + logEx.Message,
                            "Uyarı",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }

                txtSifre.Clear();

                Form sonrakiForm = string.Equals(rolAdi, Veritabani.RolAdmin, StringComparison.OrdinalIgnoreCase)
                    ? new FrmAdmin(adSoyad)
                    : new FrmAnaPanel(kullaniciId, adSoyad, rolAdi);

                sonrakiForm.FormClosed += (_, _) =>
                {
                    if (IsDisposed)
                    {
                        return;
                    }

                    Show();
                    Activate();
                    BringToFront();
                };

                // Giriş formu, yeni pencere öne alındıktan sonra gizlenir.
                // Önce gizlenirse Windows uygulamayı görev çubuğundan siler ve pencereyi arkaya atar.
                UiTema.GosterVeDevret(this, sonrakiForm);
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
