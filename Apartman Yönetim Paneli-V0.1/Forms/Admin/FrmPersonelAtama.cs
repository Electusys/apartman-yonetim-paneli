namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Apartman yöneticisinin teknisyen hesabı açtığı ekran.
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
                "Yalnızca teknisyen hesabı oluşturabilirsiniz. Aidat, ödeme ve gider işlemleri yönetici panelindedir.",
                kullanici,
                btnGeri);

            cmbRol.Items.Clear();
            cmbRol.Items.Add(Veritabani.RolTeknisyen);
            cmbRol.SelectedIndex = 0;
            cmbRol.Enabled = false;

            UiTema.Tablo(dgvPersonel);
            PersonelleriYukle();
        }

        private void FrmPersonelAtama_Load(object? sender, EventArgs e)
        {
            // Designer bağlantısı için tutuluyor; hazırlık kurucuda.
        }

        private void btnPersonelOlustur_Click(object? sender, EventArgs e)
        {
            try
            {
                IslemSonucu sonuc = KullaniciServisi.Olustur(
                    txtAd.Text,
                    txtSoyad.Text,
                    txtKullaniciAdi.Text,
                    txtSifre.Text,
                    txtSifreTekrar.Text,
                    Veritabani.RolTeknisyen,
                    "Yönetici yalnızca teknisyen atayabilir.",
                    Veritabani.RolTeknisyen);

                if (!sonuc.Basarili)
                {
                    FormIskeleti.Uyari(sonuc.Mesaj);
                    return;
                }

                FormIskeleti.Bilgi($"{Veritabani.RolTeknisyen} hesabı oluşturuldu.");
                txtAd.Clear();
                txtSoyad.Clear();
                txtKullaniciAdi.Clear();
                txtSifre.Clear();
                txtSifreTekrar.Clear();
                cmbRol.SelectedIndex = 0;
                PersonelleriYukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Personel oluşturulurken bir hata oluştu");
            }
        }

        private void PersonelleriYukle()
        {
            try
            {
                dgvPersonel.DataSource = KullaniciServisi.RolleListele(Veritabani.RolTeknisyen);
                KullaniciServisi.KullaniciGridBasliklari(dgvPersonel);
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Personel listesi yüklenemedi");
            }
        }

        private void btnGeri_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
