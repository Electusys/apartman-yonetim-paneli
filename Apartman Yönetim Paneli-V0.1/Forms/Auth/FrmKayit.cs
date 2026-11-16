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
            try
            {
                IslemSonucu sonuc = KullaniciServisi.Olustur(
                    txtAd.Text,
                    txtSoyad.Text,
                    txtKullaniciAdi.Text,
                    txtSifre.Text,
                    txtSifreTekrar.Text,
                    Veritabani.RolSakin);

                if (!sonuc.Basarili)
                {
                    FormIskeleti.Uyari(sonuc.Mesaj);
                    return;
                }

                FormIskeleti.Bilgi("Sakin hesabınız oluşturuldu. Giriş ekranından oturum açabilirsiniz.");
                Close();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Kayıt sırasında bir hata oluştu");
            }
        }

        private void btnGirisDon_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
