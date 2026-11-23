namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Salt okunur sakin rehberi. Aidat, borç ve kimlik bilgisi sorgulanmaz.
    /// </summary>
    public class FrmSakinRehberi : Form
    {
        private readonly int _kullaniciId;
        private readonly TextBox _txtAra;
        private readonly DataGridView _dgv;

        public FrmSakinRehberi(int kullaniciId, string adSoyad)
        {
            _kullaniciId = kullaniciId;
            Panel govde = FormIskeleti.Olustur(
                this,
                adSoyad,
                "Sakin Rehberi",
                "Yalnızca ad, soyad, daire ve acil iletişim görünür. Kayıt eklenemez, düzenlenemez veya silinemez.");

            _txtAra = FormIskeleti.Metin();
            _txtAra.MaxLength = 50;
            Button btnAra = FormIskeleti.Birincil("Listele");
            btnAra.Click += (_, _) => Yukle();
            Label uyari = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = UiTema.YaziSoluk,
                Font = new Font("Segoe UI", 9F),
                Text = "Bu ekran salt okunurdur. Aidat, borç ve kimlik numarası burada yer almaz."
            };

            TableLayoutPanel form = GuvenlikOrtak.Alan();
            GuvenlikOrtak.Satir(form, "Ara", _txtAra);
            GuvenlikOrtak.Satir(form, "", btnAra);
            GuvenlikOrtak.Satir(form, "", uyari);

            _dgv = FormIskeleti.Tablo();
            _dgv.ReadOnly = true;
            _dgv.AllowUserToAddRows = false;
            _dgv.AllowUserToDeleteRows = false;
            TableLayoutPanel tablo = GuvenlikOrtak.IkiBolum();
            tablo.Controls.Add(form, 0, 0);
            tablo.Controls.Add(_dgv, 0, 1);
            govde.Controls.Add(tablo);
            Yukle();
        }

        private void Yukle()
        {
            try
            {
                string aranan = _txtAra.Text.Trim();
                _dgv.DataSource = Veritabani.TabloGetir(
                    @"SELECT Ad, Soyad, Daire, AcilIletisim
                      FROM vw_GuvenlikSakinRehberi
                      WHERE @ara = ''
                         OR Ad LIKE @like
                         OR Soyad LIKE @like
                         OR Daire LIKE @like
                         OR AcilIletisim LIKE @like
                      ORDER BY Daire, Ad, Soyad",
                    ("@ara", aranan),
                    ("@like", "%" + aranan + "%"));
                UiTema.KolonBasliklari(_dgv, ("AcilIletisim", "Acil iletişim"));
                GuvenlikOrtak.Yaz(_kullaniciId, "Sakin rehberi", string.IsNullOrEmpty(aranan) ? "Rehber listelendi." : "Rehberde arama yapıldı.");
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Sakin rehberi yüklenemedi");
            }
        }
    }
}
