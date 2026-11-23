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
                FormIskeleti.Hata(ex, "Admin hesabı hazırlanırken bir hata oluştu");
            }
        }

        private void btnGirisYap_Click(object? sender, EventArgs e)
        {
            try
            {
                IslemSonucu<OturumBilgisi> sonuc = KullaniciServisi.GirisYap(txtKullaniciAdi.Text, txtSifre.Text);
                if (!sonuc.Basarili || sonuc.Deger is null)
                {
                    FormIskeleti.Uyari(sonuc.Mesaj);
                    return;
                }

                OturumBilgisi oturum = sonuc.Deger;
                Hide();
                txtSifre.Clear();

                Form sonrakiForm = string.Equals(oturum.RolAdi, Veritabani.RolAdmin, StringComparison.OrdinalIgnoreCase)
                    ? new FrmAdmin(oturum.AdSoyad)
                    : new FrmAnaPanel(oturum.KullaniciId, oturum.AdSoyad, oturum.RolAdi);

                sonrakiForm.FormClosed += (_, _) =>
                {
                    Show();
                    Activate();
                    BringToFront();
                };
                sonrakiForm.Show();
                sonrakiForm.Activate();
                sonrakiForm.BringToFront();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Giriş sırasında bir hata oluştu");
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
