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
            UiTema.SolSeritKur(
                pnlSol,
                "Admin Paneli",
                "Yönetici rolünü yalnızca Admin atar. Teknisyen ataması ve finansal işlemler yönetici panelinden yapılır.",
                kullanici,
                btnCikis);

            YoneticileriYukle();
        }

        private void btnYoneticiOlustur_Click(object? sender, EventArgs e)
        {
            try
            {
                IslemSonucu sonuc = KullaniciServisi.Olustur(
                    txtAd.Text,
                    txtSoyad.Text,
                    txtKullaniciAdi.Text,
                    txtSifre.Text,
                    txtSifreTekrar.Text,
                    Veritabani.RolYonetici);

                if (!sonuc.Basarili)
                {
                    FormIskeleti.Uyari(sonuc.Mesaj);
                    return;
                }

                FormIskeleti.Bilgi("Apartman yöneticisi hesabı oluşturuldu.");
                txtAd.Clear();
                txtSoyad.Clear();
                txtKullaniciAdi.Clear();
                txtSifre.Clear();
                txtSifreTekrar.Clear();
                YoneticileriYukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Yönetici oluşturulurken bir hata oluştu");
            }
        }

        private void YoneticileriYukle()
        {
            try
            {
                dgvYoneticiler.DataSource = KullaniciServisi.RolleListele(Veritabani.RolYonetici);
                KullaniciServisi.KullaniciGridBasliklari(dgvYoneticiler);
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Yönetici listesi yüklenemedi");
            }
        }

        private void btnCikis_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
