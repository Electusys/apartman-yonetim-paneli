namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Rol paneli: solda dikey navigasyon, sağda analiz kartları ve görev listesi.
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly int _kullaniciId;
        private readonly string _adSoyad;
        private readonly string _rolAdi;

        public Form1() : this(0, "Kullanıcı", "Apartman Sakini")
        {
        }

        public Form1(int kullaniciId, string adSoyad, string rolAdi)
        {
            _kullaniciId = kullaniciId;
            _adSoyad = adSoyad;
            _rolAdi = rolAdi;
            InitializeComponent();
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            UiTema.FormAyarla(this);
            tableAna.BackColor = UiTema.ArkaPlan;
            pnlSag.BackColor = UiTema.ArkaPlan;
            lblBaslik.ForeColor = UiTema.Yazi;
            lblAltBaslik.ForeColor = UiTema.YaziSoluk;
            lblBaslik.Text = $"Hoş geldiniz, {_adSoyad}";
            lblAltBaslik.Text = $"Aktif rol: {_rolAdi}";

            // Karşılama kartını gizle; alan analiz paneline bırakılır.
            pnlKart.Visible = false;
            tableIcerik.RowStyles[2].SizeType = SizeType.Absolute;
            tableIcerik.RowStyles[2].Height = 0;

            SolMenuyuKur();
            DashboardKur();
        }

        private bool Rol(string rol) => string.Equals(_rolAdi, rol, StringComparison.OrdinalIgnoreCase);

        private void SolMenuyuKur()
        {
            pnlSol.Controls.Clear();
            pnlSol.BackColor = UiTema.Yuzey;
            pnlSol.Padding = new Padding(18);

            TableLayoutPanel sol = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = UiTema.Yuzey
            };
            sol.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            sol.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            sol.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            sol.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            FlowLayoutPanel nav = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0, 10, 0, 8),
                BackColor = Color.Transparent
            };

            if (Rol(Veritabani.RolYonetici))
            {
                Nav(nav, "⌂", "Blok ve Daire", () => new FrmBlokDaire(_adSoyad));
                Nav(nav, "●", "Sakin Yönetimi", () => new FrmSakinYonetimi(_adSoyad));
                Nav(nav, "₺", "Aidat Yönetimi", () => new FrmAidatYonetimi(_kullaniciId, _adSoyad));
                Nav(nav, "◆", "Ödeme Yönetimi", () => new FrmOdemeYonetimi(_kullaniciId, _adSoyad));
                Nav(nav, "▣", "Gider Yönetimi", () => new FrmGiderYonetimi(_kullaniciId, _adSoyad));
                Nav(nav, "✉", "Duyuru Yönetimi", () => new FrmDuyurular(_kullaniciId, _adSoyad, _rolAdi));
                Nav(nav, "⚙", "Arıza ve Bakım", () => new FrmAriza(_kullaniciId, _adSoyad, _rolAdi));
                Nav(nav, "＋", "Personel Ata", () => new FrmPersonelAtama(_adSoyad));
            }
            else if (Rol(Veritabani.RolMuhasebeci))
            {
                Nav(nav, "◆", "Ödeme Yönetimi", () => new FrmOdemeYonetimi(_kullaniciId, _adSoyad));
                Nav(nav, "▣", "Gider Yönetimi", () => new FrmGiderYonetimi(_kullaniciId, _adSoyad));
            }
            else if (Rol(Veritabani.RolTeknisyen))
            {
                Nav(nav, "⚙", "Arıza ve Bakım", () => new FrmAriza(_kullaniciId, _adSoyad, _rolAdi));
            }
            else
            {
                Nav(nav, "✉", "Duyurular", () => new FrmDuyurular(_kullaniciId, _adSoyad, _rolAdi));
                Nav(nav, "₺", "Aidat Bilgilerim", () => new FrmAidatlarim(_kullaniciId, _adSoyad));
                Nav(nav, "⚙", "Arıza Bildirimi", () => new FrmAriza(_kullaniciId, _adSoyad, _rolAdi));
            }

            Label kullanici = new Label
            {
                Text = _adSoyad,
                Dock = DockStyle.Top,
                Height = 24,
                ForeColor = UiTema.Yazi,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold)
            };
            Label rol = new Label
            {
                Text = _rolAdi,
                Dock = DockStyle.Top,
                Height = 22,
                ForeColor = UiTema.Vurgu,
                Font = new Font("Segoe UI", 8.5F)
            };
            Panel kimlik = new Panel { Dock = DockStyle.Top, Height = 54, BackColor = Color.Transparent };
            kimlik.Controls.Add(rol);
            kimlik.Controls.Add(kullanici);

            Button cikis = new Button { Text = "Çıkış Yap", Dock = DockStyle.Top };
            UiTema.IkincilButon(cikis);
            cikis.Click += btnCikis_Click;

            sol.Controls.Add(new LogoPanel(), 0, 0);
            sol.Controls.Add(nav, 0, 1);
            sol.Controls.Add(kimlik, 0, 2);
            sol.Controls.Add(cikis, 0, 3);
            pnlSol.Controls.Add(sol);
        }

        private void Nav(FlowLayoutPanel nav, string ikon, string metin, Func<Form> olustur)
        {
            Button button = UiTema.NavButonu(ikon, metin);
            button.Dock = DockStyle.None;
            button.Width = Math.Max(190, pnlSol.ClientSize.Width - 44);
            button.Margin = new Padding(0, 0, 0, 6);
            button.Click += (_, _) =>
            {
                using Form form = olustur();
                form.ShowDialog(this);
                DashboardKur();
            };
            nav.Controls.Add(button);
        }

        private void DashboardKur()
        {
            foreach (Control c in tableIcerik.Controls.OfType<Control>().Where(x => x.Name == "pnlDashboard").ToList())
            {
                tableIcerik.Controls.Remove(c);
                c.Dispose();
            }

            TableLayoutPanel dash = new TableLayoutPanel
            {
                Name = "pnlDashboard",
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 2,
                Margin = new Padding(0, 8, 0, 0)
            };
            dash.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            dash.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            dash.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            dash.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            dash.RowStyles.Add(new RowStyle(SizeType.Absolute, 132F));
            dash.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            string blok = "—", daire = "—", sakin = "—", ariza = "—";
            DataGridView liste = FormIskeleti.Tablo();
            try
            {
                blok = Veritabani.Sayi("SELECT COUNT(*) FROM Bloklar").ToString();
                daire = Veritabani.Sayi("SELECT COUNT(*) FROM Daireler").ToString();
                sakin = Veritabani.Sayi("SELECT COUNT(*) FROM Sakinler WHERE Aktif = 1").ToString();
                ariza = Veritabani.Sayi("SELECT COUNT(*) FROM ArizaBildirimleri WHERE Durum <> @d", ("@d", Veritabani.ArizaTamamlandi)).ToString();
                liste.DataSource = Veritabani.TabloGetir(
                    @"SELECT Baslik AS Görev, DATE_FORMAT(YayimTarihi, '%d.%m.%Y') AS Tarih
                      FROM Duyurular
                      ORDER BY YayimTarihi DESC
                      LIMIT 8");
            }
            catch
            {
                liste.DataSource = null;
            }

            dash.Controls.Add(UiTema.AnalizKarti("Blok", blok, "Kayıtlı blok"), 0, 0);
            dash.Controls.Add(UiTema.AnalizKarti("Daire", daire, "Toplam daire"), 1, 0);
            dash.Controls.Add(UiTema.AnalizKarti("Sakin", sakin, "Aktif sakin"), 2, 0);
            dash.Controls.Add(UiTema.AnalizKarti("Açık arıza", ariza, "Bekleyen iş"), 3, 0);

            Panel gorev = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UiTema.Kart,
                Margin = new Padding(0, 8, 14, 0),
                Padding = new Padding(16)
            };
            UiTema.Yuvarla(gorev, 16);
            Label gorevBaslik = new Label
            {
                Text = "SON DUYURULAR / GÖREVLER",
                Dock = DockStyle.Top,
                Height = 28,
                ForeColor = UiTema.Vurgu,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold)
            };
            gorev.Controls.Add(liste);
            gorev.Controls.Add(gorevBaslik);
            dash.SetColumnSpan(gorev, 4);
            dash.Controls.Add(gorev, 0, 1);

            tableIcerik.Controls.Add(dash, 0, 3);
        }

        private void btnPersonel_Click(object? sender, EventArgs e)
        {
            using FrmPersonelAtama personelFormu = new FrmPersonelAtama(_adSoyad);
            personelFormu.ShowDialog(this);
            DashboardKur();
        }

        private void btnCikis_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
