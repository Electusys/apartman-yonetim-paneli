using MySqlConnector;

namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Herkese açık kayıt ekranı. Buradan yalnızca "Apartman Sakini" hesabı açılır.
    /// </summary>
    public partial class FrmKayit : Form
    {
        private CheckBox? _kvkkOnay;

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

            _kvkkOnay = new CheckBox
            {
                Text = "KVKK aydınlatma metnini okudum.",
                AutoSize = true,
                ForeColor = UiTema.Yazi,
                Location = new Point(0, 4)
            };
            Button btnKvkk = new Button
            {
                Text = "Metni oku",
                Location = new Point(0, 32),
                Size = new Size(150, 36)
            };
            UiTema.IkincilButon(btnKvkk);
            btnKvkk.Click += (_, _) =>
            {
                using FrmKvkk kvkk = new FrmKvkk();
                kvkk.ShowDialog(this);
            };
            Panel kvkkAlani = new Panel { Dock = DockStyle.Fill };
            kvkkAlani.Controls.Add(btnKvkk);
            kvkkAlani.Controls.Add(_kvkkOnay);
            tableForm.Controls.Add(kvkkAlani, 0, 9);
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

            if (_kvkkOnay is not { Checked: true })
            {
                MessageBox.Show("Kayıt için KVKK aydınlatma metnini okuyup onay kutusunu işaretleyin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string ad = txtAd.Text.Trim();
            string soyad = txtSoyad.Text.Trim();
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Text;

            if (string.Equals(kullaniciAdi, Veritabani.AdminKullaniciAdi, StringComparison.OrdinalIgnoreCase)
                || string.Equals(kullaniciAdi, Veritabani.GuvenlikKullaniciAdi, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Bu kullanıcı adı sistem tarafından ayrılmıştır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (Hesaplar.KullaniciVar(kullaniciAdi))
                {
                    MessageBox.Show("Bu kullanıcı adı zaten kayıtlı. Lütfen başka bir kullanıcı adı seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Hesaplar.KullaniciEkle(ad, soyad, kullaniciAdi, sifre, Veritabani.RolSakin);

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