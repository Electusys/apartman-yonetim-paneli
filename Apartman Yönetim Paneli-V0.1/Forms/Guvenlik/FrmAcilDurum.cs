namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Polis ve itfaiye düğmeleri yalnızca kağıt kayıt oluşturur.
    /// Kolluk veya itfaiyeye otomatik bildirim gönderilmez.
    /// </summary>
    public class FrmAcilDurum : Form
    {
        private readonly int _kullaniciId;
        private readonly TextBox _txtNot;
        private readonly DataGridView _dgv;

        public FrmAcilDurum(int kullaniciId, string adSoyad)
        {
            _kullaniciId = kullaniciId;
            Panel govde = FormIskeleti.Olustur(
                this,
                adSoyad,
                "Acil Durum",
                "Polis ve İtfaiye düğmeleri arama yapmaz. Yalnızca kağıt kayıt ve sistem günlüğü oluşturur.");

            _txtNot = FormIskeleti.Metin();
            _txtNot.MaxLength = 300;
            Button btnPolis = FormIskeleti.Birincil("Polis");
            btnPolis.Click += (_, _) => Kaydet("Polis");
            Button btnItfaiye = FormIskeleti.Birincil("İtfaiye");
            btnItfaiye.Click += (_, _) => Kaydet("İtfaiye");
            FlowLayoutPanel dugmeler = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                WrapContents = false
            };
            dugmeler.Controls.Add(btnPolis);
            dugmeler.Controls.Add(btnItfaiye);
            Label uyari = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = UiTema.YaziSoluk,
                Text = "Gerçek bir birime bildirim gitmez. Olayı ayrıca kağıt forma yazın."
            };

            TableLayoutPanel form = GuvenlikOrtak.Alan();
            GuvenlikOrtak.Satir(form, "Kısa not", _txtNot);
            GuvenlikOrtak.Satir(form, "", dugmeler);
            GuvenlikOrtak.Satir(form, "", uyari);

            _dgv = FormIskeleti.Tablo();
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
                _dgv.DataSource = Veritabani.TabloGetir(
                    @"SELECT a.AcilID, a.Tur, a.NotMetni, a.OlusturmaTarihi,
                             CONCAT(k.Ad, ' ', k.Soyad) AS Personel
                      FROM AcilDurumKayitlari a
                      INNER JOIN Kullanicilar k ON k.KullaniciID = a.OlusturanKullaniciID
                      ORDER BY a.AcilID DESC");
                UiTema.KolonBasliklari(_dgv,
                    ("AcilID", "ID"),
                    ("NotMetni", "Not"),
                    ("OlusturmaTarihi", "Tarih ve saat"));
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Acil durum kayıtları yüklenemedi");
            }
        }

        private void Kaydet(string tur)
        {
            string? not = string.IsNullOrWhiteSpace(_txtNot.Text) ? null : _txtNot.Text.Trim();
            try
            {
                Veritabani.Calistir(
                    @"INSERT INTO AcilDurumKayitlari (Tur, NotMetni, OlusturanKullaniciID)
                      VALUES (@tur, @not, @kullanici)",
                    ("@tur", tur),
                    ("@not", not),
                    ("@kullanici", _kullaniciId));
                GuvenlikOrtak.Yaz(_kullaniciId, "Acil durum", tur + " kağıt kaydı oluşturuldu. Dış bildirim gönderilmedi.");
                _txtNot.Clear();
                FormIskeleti.Bilgi(tur + " kaydı yazıldı. Arama yapılmadı; kağıt forma da işleyin.");
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Acil durum kaydı oluşturulamadı");
            }
        }
    }
}
