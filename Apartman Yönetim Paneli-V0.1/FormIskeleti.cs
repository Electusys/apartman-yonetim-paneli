namespace Apartman_Yönetim_Paneli_V0._1
{
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
