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
            button.FlatAppearance.MouseOverBackColor = VurguKoyu;
            button.FlatAppearance.MouseDownBackColor = Vurgu;
            button.Cursor = Cursors.Hand;
            button.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            button.Height = 44;
            button.UseVisualStyleBackColor = false;
            Yuvarla(button, 10);
        }

        public static void IkincilButon(Button button)
        {
            button.BackColor = Kart;
            button.ForeColor = Yazi;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Kenar;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 64, 84);
            button.Cursor = Cursors.Hand;
            button.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            button.Height = 42;
            button.UseVisualStyleBackColor = false;
            Yuvarla(button, 10);
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
                BackColor = Color.Transparent,
                ForeColor = Yazi,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI Semibold", 10F),
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Vurgu;
            button.FlatAppearance.MouseDownBackColor = VurguKoyu;
            button.MouseEnter += (_, _) => button.ForeColor = YaziKoyu;
            button.MouseLeave += (_, _) => button.ForeColor = Yazi;
            Yuvarla(button, 10);
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

    /// <summary>
    /// Sol dikey menü (logo + geri) ve sağda esnek içerik alanı.
    /// </summary>
    internal static class FormIskeleti
    {
        public static Panel Olustur(Form form, string kullaniciAdSoyad, string baslik, string aciklama)
        {
            form.Text = baslik;
            form.ClientSize = new Size(1180, 740);
            form.AutoScaleMode = AutoScaleMode.Font;
            UiTema.FormAyarla(form);

            TableLayoutPanel tableAna = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = UiTema.ArkaPlan
            };
            tableAna.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
            tableAna.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 76F));
            tableAna.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            TableLayoutPanel sol = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = UiTema.Yuzey,
                Padding = new Padding(18),
                ColumnCount = 1,
                RowCount = 5
            };
            sol.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            sol.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            sol.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            sol.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            sol.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            LogoPanel logo = new LogoPanel();
            Label lblSayfa = new Label
            {
                Text = baslik,
                Dock = DockStyle.Top,
                Height = 56,
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = UiTema.Yazi,
                Padding = new Padding(4, 12, 4, 0)
            };
            Label lblAciklama = new Label
            {
                Text = aciklama,
                Dock = DockStyle.Fill,
                ForeColor = UiTema.YaziSoluk,
                Font = new Font("Segoe UI", 9F),
                Padding = new Padding(4, 8, 4, 8)
            };
            Label lblKullanici = new Label
            {
                Text = kullaniciAdSoyad,
                Dock = DockStyle.Top,
                Height = 36,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = UiTema.Vurgu,
                TextAlign = ContentAlignment.MiddleLeft
            };
            Button btnGeri = new Button { Text = "←  Geri", Dock = DockStyle.Top };
            UiTema.IkincilButon(btnGeri);
            btnGeri.Click += (_, _) => form.Close();

            sol.Controls.Add(logo, 0, 0);
            sol.Controls.Add(lblSayfa, 0, 1);
            sol.Controls.Add(lblAciklama, 0, 2);
            sol.Controls.Add(lblKullanici, 0, 3);
            sol.Controls.Add(btnGeri, 0, 4);

            Panel pnlSag = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UiTema.ArkaPlan,
                Padding = new Padding(28, 24, 28, 24)
            };

            TableLayoutPanel tableIcerik = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };
            tableIcerik.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableIcerik.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tableIcerik.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableIcerik.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Label lblBaslik = new Label
            {
                Text = baslik,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold),
                ForeColor = UiTema.Yazi
            };
            Label lblAltBaslik = new Label
            {
                Text = aciklama,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                ForeColor = UiTema.YaziSoluk
            };
            Panel pnlGovde = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            tableIcerik.Controls.Add(lblBaslik, 0, 0);
            tableIcerik.Controls.Add(lblAltBaslik, 0, 1);
            tableIcerik.Controls.Add(pnlGovde, 0, 2);
            pnlSag.Controls.Add(tableIcerik);

            tableAna.Controls.Add(sol, 0, 0);
            tableAna.Controls.Add(pnlSag, 1, 0);
            form.Controls.Add(tableAna);
            return pnlGovde;
        }

        public static Label Etiket(string metin)
        {
            Label label = new Label
            {
                Text = metin,
                AutoSize = true,
                Margin = new Padding(0, 8, 0, 4)
            };
            UiTema.Etiket(label);
            return label;
        }

        public static TextBox Metin(bool sifre = false)
        {
            TextBox kutu = new TextBox { Dock = DockStyle.Fill, Height = 32 };
            UiTema.MetinKutusu(kutu);
            if (sifre)
            {
                kutu.UseSystemPasswordChar = true;
            }

            return kutu;
        }

        public static ComboBox Combo()
        {
            ComboBox combo = new ComboBox { Dock = DockStyle.Fill, Height = 32 };
            UiTema.Combo(combo);
            return combo;
        }

        public static DateTimePicker Tarih(string? ozelFormat = null)
        {
            DateTimePicker dtp = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 11F),
                CalendarForeColor = UiTema.Yazi,
                CalendarMonthBackground = UiTema.Yuzey
            };
            if (!string.IsNullOrWhiteSpace(ozelFormat))
            {
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = ozelFormat;
                dtp.ShowUpDown = true;
            }

            return dtp;
        }

        public static NumericUpDown Para()
        {
            NumericUpDown kutu = new NumericUpDown { Dock = DockStyle.Fill, Height = 32 };
            UiTema.ParaKutusu(kutu);
            return kutu;
        }

        public static DataGridView Tablo()
        {
            DataGridView grid = new DataGridView { Dock = DockStyle.Fill };
            UiTema.Tablo(grid);
            return grid;
        }

        public static Button Birincil(string metin)
        {
            Button button = new Button { Text = metin, Width = 220, Height = 44 };
            UiTema.BirincilButon(button);
            return button;
        }

        public static void Hata(Exception ex, string baslik)
        {
            MessageBox.Show(baslik + ":\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Uyari(string mesaj)
        {
            MessageBox.Show(mesaj, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Bilgi(string mesaj)
        {
            MessageBox.Show(mesaj, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
