using System.Drawing.Drawing2D;

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
}
