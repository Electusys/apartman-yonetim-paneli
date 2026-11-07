using System.Drawing.Drawing2D;
using System.Reflection;

namespace Apartman_Yönetim_Paneli_V0._1
{

    /// <summary>
    /// Antrasit koyu tema + fıstık yeşili vurgu. Düz (flat) kontroller, yuvarlatılmış köşeler.
    /// </summary>
    internal static class UiTema
    {
        public static readonly Color ArkaPlan = Color.FromArgb(31, 34, 43);      // #1f222b
        public static readonly Color Yuzey = Color.FromArgb(42, 46, 61);         // #2a2e3d
        public static readonly Color Kart = Color.FromArgb(50, 55, 72);
        public static readonly Color Vurgu = Color.FromArgb(197, 225, 29);       // #c5e11d
        public static readonly Color VurguKoyu = Color.FromArgb(212, 236, 29);    // #d4ec1d
        public static readonly Color Yazi = Color.FromArgb(236, 237, 232);
        public static readonly Color YaziSoluk = Color.FromArgb(154, 160, 176);
        public static readonly Color Kenar = Color.FromArgb(61, 66, 84);
        public static readonly Color Tehlike = Color.FromArgb(248, 113, 113);
        public static readonly Color YaziKoyu = Color.FromArgb(31, 34, 43);

        public static void FormAyarla(Form form)
        {
            form.BackColor = ArkaPlan;
            form.ForeColor = Yazi;
            form.Font = new Font("Segoe UI", 10F);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimumSize = new Size(980, 620);
            form.FormBorderStyle = FormBorderStyle.Sizable;
            form.MaximizeBox = true;
            CiftTampon(form);
            YumusakForm(form);
        }

        public static void Yuvarla(Control kontrol, int yaricap = 12)
        {
            void Uygula()
            {
                if (kontrol.Width < 4 || kontrol.Height < 4)
                {
                    return;
                }

                using GraphicsPath yol = YuvarlakYol(new Rectangle(0, 0, kontrol.Width, kontrol.Height), yaricap);
                kontrol.Region?.Dispose();
                kontrol.Region = new Region(yol);
            }

            kontrol.Resize += (_, _) => Uygula();
            Uygula();
        }

        public static GraphicsPath YuvarlakYol(Rectangle r, int yaricap)
        {
            int d = Math.Max(2, yaricap * 2);
            GraphicsPath yol = new GraphicsPath();
            yol.AddArc(r.X, r.Y, d, d, 180, 90);
            yol.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            yol.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            yol.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            yol.CloseFigure();
            return yol;
        }

        public static void BirincilButon(Button button)
        {
            button.BackColor = Vurgu;
            button.ForeColor = YaziKoyu;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
            button.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            button.Height = 44;
            button.UseVisualStyleBackColor = false;
            Yuvarla(button, 10);
            YumusakButon(button, Vurgu, VurguKoyu, Color.FromArgb(154, 184, 16), YaziKoyu, YaziKoyu);
        }

        public static void IkincilButon(Button button)
        {
            button.BackColor = Kart;
            button.ForeColor = Yazi;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Kenar;
            button.FlatAppearance.BorderSize = 1;
            button.Cursor = Cursors.Hand;
            button.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            button.Height = 42;
            button.UseVisualStyleBackColor = false;
            Yuvarla(button, 10);
            YumusakButon(
                button,
                Kart,
                Color.FromArgb(64, 70, 90),
                Color.FromArgb(38, 42, 56),
                Yazi,
                Yazi);
        }

        public static Button NavButonu(string ikon, string metin)
        {
            Button button = new Button
            {
                Text = $"  {ikon}   {metin}",
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Top,
                Height = 48,
                Margin = new Padding(0, 0, 0, 6),
                BackColor = Yuzey,
                ForeColor = Yazi,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI Semibold", 10F),
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderSize = 0;
            Yuvarla(button, 10);
            YumusakButon(button, Yuzey, Vurgu, VurguKoyu, Yazi, YaziKoyu);
            return button;
        }

        public static void MetinKutusu(TextBox textBox)
        {
            textBox.BackColor = Yuzey;
            textBox.ForeColor = Yazi;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = new Font("Segoe UI", 11F);
        }

        public static void Etiket(Label label, bool baslik = false)
        {
            label.ForeColor = baslik ? Yazi : YaziSoluk;
            label.BackColor = Color.Transparent;
        }

        public static void Tablo(DataGridView grid)
        {
            grid.BackgroundColor = Kart;
            grid.BorderStyle = BorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.GridColor = Kenar;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.DefaultCellStyle.BackColor = Kart;
            grid.DefaultCellStyle.ForeColor = Yazi;
            grid.DefaultCellStyle.SelectionBackColor = Vurgu;
            grid.DefaultCellStyle.SelectionForeColor = YaziKoyu;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Yuzey;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Vurgu;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            grid.ColumnHeadersHeight = 40;
            grid.RowTemplate.Height = 36;
        }

        public static void Combo(ComboBox combo)
        {
            combo.BackColor = Yuzey;
            combo.ForeColor = Yazi;
            combo.FlatStyle = FlatStyle.Flat;
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.Font = new Font("Segoe UI", 11F);
        }

        public static void ParaKutusu(NumericUpDown kutu)
        {
            kutu.BackColor = Yuzey;
            kutu.ForeColor = Yazi;
            kutu.BorderStyle = BorderStyle.FixedSingle;
            kutu.DecimalPlaces = 2;
            kutu.Maximum = 1000000;
            kutu.ThousandsSeparator = true;
            kutu.Font = new Font("Segoe UI", 11F);
        }

        public static void KolonBasliklari(DataGridView grid, params (string Kolon, string Baslik)[] esleme)
        {
            foreach ((string kolon, string baslik) in esleme)
            {
                if (grid.Columns[kolon] is DataGridViewColumn sutun)
                {
                    sutun.HeaderText = baslik;
                }
            }
        }

        public static Button MenuButonu(string metin) => NavButonu("●", metin);

        /// <summary>
        /// Giriş/Admin/Kayıt sol şeridini: logo + başlık + açıklama (+ alt kontroller) olarak kurar.
        /// </summary>
        public static void SolSeritKur(Panel pnlSol, string baslik, string aciklama, params Control[] altKontroller)
        {
            pnlSol.Controls.Clear();
            pnlSol.BackColor = Yuzey;
            pnlSol.Padding = new Padding(22);

            TableLayoutPanel sol = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3 + altKontroller.Length,
                BackColor = Yuzey
            };
            sol.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            sol.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            sol.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            for (int i = 0; i < altKontroller.Length; i++)
            {
                sol.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            LogoPanel logo = new LogoPanel();
            Label lblBaslik = new Label
            {
                Text = baslik,
                Dock = DockStyle.Top,
                AutoSize = false,
                Height = 64,
                Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold),
                ForeColor = Yazi,
                Padding = new Padding(2, 14, 2, 0)
            };
            Label lblAciklama = new Label
            {
                Text = aciklama,
                Dock = DockStyle.Fill,
                ForeColor = YaziSoluk,
                Font = new Font("Segoe UI", 9.5F),
                Padding = new Padding(2, 8, 2, 8)
            };

            sol.Controls.Add(logo, 0, 0);
            sol.Controls.Add(lblBaslik, 0, 1);
            sol.Controls.Add(lblAciklama, 0, 2);
            for (int i = 0; i < altKontroller.Length; i++)
            {
                Control c = altKontroller[i];
                c.Dock = DockStyle.Top;
                sol.Controls.Add(c, 0, 3 + i);
            }

            pnlSol.Controls.Add(sol);
        }

        public static Panel AnalizKarti(string baslik, string deger, string not)
        {
            Panel kart = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Kart,
                Margin = new Padding(0, 0, 14, 14),
                Padding = new Padding(18)
            };
            Yuvarla(kart, 16);

            Label lblNot = new Label
            {
                Text = not,
                Dock = DockStyle.Bottom,
                Height = 22,
                ForeColor = YaziSoluk,
                Font = new Font("Segoe UI", 8.5F)
            };
            Label lblDeger = new Label
            {
                Text = deger,
                Dock = DockStyle.Fill,
                ForeColor = Vurgu,
                Font = new Font("Segoe UI Semibold", 26F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            Label lblBaslik = new Label
            {
                Text = baslik.ToUpperInvariant(),
                Dock = DockStyle.Top,
                Height = 22,
                ForeColor = YaziSoluk,
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold)
            };
            kart.Controls.Add(lblDeger);
            kart.Controls.Add(lblNot);
            kart.Controls.Add(lblBaslik);
            return kart;
        }

        private static readonly Dictionary<Button, ButonAkis> Butonlar = new();
        private static readonly Dictionary<Form, FormAkis> Formlar = new();
        private static readonly HashSet<Form> KapanisIzinli = new();
        private static readonly System.Windows.Forms.Timer Kadran = new() { Interval = 16 };
        private static bool _kadranBagli;

        private static void CiftTampon(Control kontrol)
        {
            typeof(Control)
                .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.SetValue(kontrol, true);
        }

        /// <summary>
        /// Ekran açılış ve kapanışında kısa bir opaklık geçişi.
        /// </summary>
        private static void YumusakForm(Form form)
        {
            if (Formlar.ContainsKey(form))
            {
                return;
            }

            form.Opacity = 0;
            Formlar[form] = new FormAkis { Hedef = 1 };
            form.Shown += (_, _) => OpakligaGit(form, 1, null);
            form.VisibleChanged += (_, _) =>
            {
                if (form.IsDisposed || form.Disposing)
                {
                    return;
                }

                if (!form.Visible)
                {
                    if (!KapanisIzinli.Contains(form))
                    {
                        OpaklikYaz(form, 0);
                    }

                    return;
                }

                if (form.Opacity < 0.98)
                {
                    OpakligaGit(form, 1, null);
                }
            };
            form.FormClosing += (_, e) =>
            {
                if (KapanisIzinli.Contains(form) || form.Opacity <= 0.04)
                {
                    KapanisIzinli.Add(form);
                    return;
                }

                e.Cancel = true;
                OpakligaGit(form, 0, () =>
                {
                    KapanisIzinli.Add(form);
                    if (!form.IsDisposed)
                    {
                        form.Close();
                    }
                });
            };
            form.FormClosed += (_, _) =>
            {
                Formlar.Remove(form);
                KapanisIzinli.Remove(form);
            };
        }

        private static void OpakligaGit(Form form, double hedef, Action? bitis)
        {
            if (form.IsDisposed)
            {
                return;
            }

            if (!Formlar.TryGetValue(form, out FormAkis? akis))
            {
                akis = new FormAkis();
                Formlar[form] = akis;
            }

            akis.Hedef = hedef;
            akis.Bitis = bitis;
            KadraniBaslat();
        }

        /// <summary>
        /// Üzerine gelince ve basınca rengin yumuşakça kayması.
        /// </summary>
        private static void YumusakButon(Button button, Color normal, Color uzerinde, Color basili, Color yazi, Color yaziUzerinde)
        {
            ButonAkis akis = new()
            {
                Normal = normal,
                Uzerinde = uzerinde,
                Basili = basili,
                YaziNormal = yazi,
                YaziUzerinde = yaziUzerinde,
                SuAn = normal,
                YaziSuAn = yazi
            };
            akis.HedefiKur();
            Butonlar[button] = akis;
            RengiUygula(button, akis);

            button.MouseEnter += (_, _) =>
            {
                akis.FareUzerinde = true;
                akis.HedefiKur();
                KadraniBaslat();
            };
            button.MouseLeave += (_, _) =>
            {
                akis.FareUzerinde = false;
                akis.BasiliMi = false;
                akis.HedefiKur();
                KadraniBaslat();
            };
            button.MouseDown += (_, _) =>
            {
                akis.BasiliMi = true;
                akis.HedefiKur();
                KadraniBaslat();
            };
            button.MouseUp += (_, _) =>
            {
                akis.BasiliMi = false;
                akis.HedefiKur();
                KadraniBaslat();
            };
            button.Disposed += (_, _) => Butonlar.Remove(button);
        }

        private static void KadraniBaslat()
        {
            if (!_kadranBagli)
            {
                _kadranBagli = true;
                Kadran.Tick += (_, _) => Kare();
            }

            if (!Kadran.Enabled)
            {
                Kadran.Start();
            }
        }

        private static void Kare()
        {
            bool suruyor = false;
            foreach ((Button button, ButonAkis akis) in Butonlar.ToList())
            {
                if (button.IsDisposed)
                {
                    Butonlar.Remove(button);
                    continue;
                }

                if (Yakin(akis.SuAn, akis.Hedef) && Yakin(akis.YaziSuAn, akis.YaziHedef))
                {
                    if (akis.SuAn != akis.Hedef || akis.YaziSuAn != akis.YaziHedef)
                    {
                        akis.SuAn = akis.Hedef;
                        akis.YaziSuAn = akis.YaziHedef;
                        RengiUygula(button, akis);
                    }

                    continue;
                }

                akis.SuAn = Yaklastir(akis.SuAn, akis.Hedef);
                akis.YaziSuAn = Yaklastir(akis.YaziSuAn, akis.YaziHedef);
                RengiUygula(button, akis);
                suruyor = true;
            }

            foreach ((Form form, FormAkis akis) in Formlar.ToList())
            {
                if (form.IsDisposed)
                {
                    Formlar.Remove(form);
                    continue;
                }

                if (!form.Visible && akis.Hedef > 0)
                {
                    continue;
                }

                double fark = akis.Hedef - form.Opacity;
                if (Math.Abs(fark) < 0.02)
                {
                    if (Math.Abs(form.Opacity - akis.Hedef) >= 0.001)
                    {
                        OpaklikYaz(form, akis.Hedef);
                    }

                    Action? bitis = akis.Bitis;
                    akis.Bitis = null;
                    bitis?.Invoke();
                    continue;
                }

                OpaklikYaz(form, Math.Clamp(form.Opacity + (fark * 0.22), 0, 1));
                suruyor = true;
            }

            foreach ((Form form, FormAkis akis) in Formlar.ToList())
            {
                if (!form.IsDisposed && form.Visible && Math.Abs(form.Opacity - akis.Hedef) >= 0.02)
                {
                    suruyor = true;
                }
            }

            if (!suruyor)
            {
                Kadran.Stop();
            }
        }

        private static void OpaklikYaz(Form form, double deger)
        {
            if (form.IsDisposed || form.Disposing)
            {
                return;
            }

            try
            {
                form.Opacity = deger;
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private static void RengiUygula(Button button, ButonAkis akis)
        {
            button.BackColor = akis.SuAn;
            button.FlatAppearance.MouseOverBackColor = akis.SuAn;
            button.FlatAppearance.MouseDownBackColor = akis.SuAn;
            button.ForeColor = akis.YaziSuAn;
        }

        private static Color Yaklastir(Color suAn, Color hedef)
        {
            if (Yakin(suAn, hedef))
            {
                return hedef;
            }

            const float adim = 0.22f;
            return Color.FromArgb(
                (int)(suAn.A + ((hedef.A - suAn.A) * adim)),
                (int)(suAn.R + ((hedef.R - suAn.R) * adim)),
                (int)(suAn.G + ((hedef.G - suAn.G) * adim)),
                (int)(suAn.B + ((hedef.B - suAn.B) * adim)));
        }

        private static bool Yakin(Color a, Color b)
        {
            return Math.Abs(a.A - b.A) + Math.Abs(a.R - b.R) + Math.Abs(a.G - b.G) + Math.Abs(a.B - b.B) < 10;
        }

        private sealed class ButonAkis
        {
            public Color Normal;
            public Color Uzerinde;
            public Color Basili;
            public Color YaziNormal;
            public Color YaziUzerinde;
            public Color SuAn;
            public Color Hedef;
            public Color YaziSuAn;
            public Color YaziHedef;
            public bool FareUzerinde;
            public bool BasiliMi;

            public void HedefiKur()
            {
                if (BasiliMi)
                {
                    Hedef = Basili;
                    YaziHedef = YaziUzerinde;
                    return;
                }

                Hedef = FareUzerinde ? Uzerinde : Normal;
                YaziHedef = FareUzerinde ? YaziUzerinde : YaziNormal;
            }
        }

        private sealed class FormAkis
        {
            public double Hedef { get; set; } = 1;
            public Action? Bitis { get; set; }
        }
    }

    /// <summary>
    /// Üst solda marka: ekteki logo görseli + AYP yazısı.
    /// </summary>
    internal sealed class LogoPanel : Panel
    {
        private static Image? _logo;
        private static readonly object LogoKilit = new();

        public LogoPanel()
        {
            DoubleBuffered = true;
            Height = 92;
            MinimumSize = new Size(180, 92);
            BackColor = Color.Transparent;
            Dock = DockStyle.Top;
            Padding = new Padding(0, 4, 0, 4);
        }

        private static Image? LogoAl()
        {
            if (_logo != null)
            {
                return _logo;
            }

            lock (LogoKilit)
            {
                if (_logo != null)
                {
                    return _logo;
                }

                string[] adaylar =
                {
                    Path.Combine(AppContext.BaseDirectory, "Assets", "logo.png"),
                    Path.Combine(AppContext.BaseDirectory, "logo.png"),
                    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "Assets", "logo.png")
                };

                foreach (string yol in adaylar)
                {
                    if (File.Exists(yol))
                    {
                        _logo = Image.FromFile(yol);
                        return _logo;
                    }
                }

                return null;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle ikon = new Rectangle(0, 8, 72, 72);
            Image? logo = LogoAl();
            if (logo != null)
            {
                using GraphicsPath maske = UiTema.YuvarlakYol(ikon, 16);
                g.SetClip(maske);
                g.DrawImage(logo, ikon);
                g.ResetClip();

                using Pen kenar = new Pen(UiTema.Kenar, 1.5f);
                g.DrawPath(kenar, maske);
            }
            else
            {
                using (GraphicsPath yol = UiTema.YuvarlakYol(ikon, 16))
                using (SolidBrush firca = new SolidBrush(UiTema.Vurgu))
                {
                    g.FillPath(firca, yol);
                }

                using Pen cerceve = new Pen(UiTema.YaziKoyu, 2.4f);
                int x = ikon.X + 22;
                int taban = ikon.Bottom - 18;
                g.DrawLine(cerceve, x, taban, x + 28, taban);
                g.DrawLine(cerceve, x, taban, x, ikon.Y + 28);
                g.DrawLine(cerceve, x + 28, taban, x + 28, ikon.Y + 28);
                g.DrawLine(cerceve, x - 5, ikon.Y + 30, x + 14, ikon.Y + 16);
                g.DrawLine(cerceve, x + 14, ikon.Y + 16, x + 33, ikon.Y + 30);
            }

            using Font fontMarka = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            using Font fontAlt = new Font("Segoe UI", 8.25F, FontStyle.Bold);
            using SolidBrush yazi = new SolidBrush(UiTema.Yazi);
            using SolidBrush vurgu = new SolidBrush(UiTema.Vurgu);
            g.DrawString("AYP", fontMarka, yazi, 84, 18);
            g.DrawString("APARTMAN YÖNETİMİ", fontAlt, vurgu, 86, 52);
        }
    }
}
