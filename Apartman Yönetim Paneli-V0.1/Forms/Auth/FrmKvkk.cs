namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Aydınlatma metnini ayrı pencerede gösterir. Metnin kendisi KvkkMetni sınıfındadır.
    /// </summary>
    internal sealed class FrmKvkk : Form
    {
        public FrmKvkk()
        {
            Text = KvkkMetni.Baslik;
            ClientSize = new Size(980, 680);
            UiTema.FormAyarla(this);

            TableLayoutPanel yerlesim = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(28),
                BackColor = UiTema.ArkaPlan
            };
            yerlesim.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            yerlesim.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            Label baslik = new Label
            {
                Text = KvkkMetni.Baslik,
                Dock = DockStyle.Top,
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold),
                ForeColor = UiTema.Yazi,
                Padding = new Padding(0, 0, 0, 12)
            };

            TextBox metin = new TextBox
            {
                Text = KvkkMetni.Govde,
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BorderStyle = BorderStyle.None,
                BackColor = UiTema.Yuzey,
                ForeColor = UiTema.Yazi,
                Font = new Font("Segoe UI", 11F)
            };

            Button kapat = new Button
            {
                Text = "Kapat",
                Dock = DockStyle.Right,
                Width = 140,
                Height = 40,
                Margin = new Padding(0, 16, 0, 0)
            };
            UiTema.BirincilButon(kapat);
            kapat.Click += (_, _) => Close();

            yerlesim.Controls.Add(baslik, 0, 0);
            yerlesim.Controls.Add(metin, 0, 1);
            yerlesim.Controls.Add(kapat, 0, 2);
            Controls.Add(yerlesim);
        }
    }
}
