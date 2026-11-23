namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Güvenlik ekranlarının ortak yerleşimi, plaka düzeni ve işlem günlüğü.
    /// </summary>
    internal static class GuvenlikOrtak
    {
        public static TableLayoutPanel IkiBolum()
        {
            TableLayoutPanel tablo = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            tablo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tablo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            return tablo;
        }

        public static TableLayoutPanel Alan()
        {
            TableLayoutPanel form = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2
            };
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68F));
            return form;
        }

        public static void Satir(TableLayoutPanel form, string etiket, Control kontrol)
        {
            int satir = form.RowCount++;
            int yukseklik = kontrol switch
            {
                FlowLayoutPanel => 64,
                Label => 72,
                _ => 40
            };
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, yukseklik));
            form.Controls.Add(string.IsNullOrEmpty(etiket) ? new Label() : FormIskeleti.Etiket(etiket), 0, satir);
            kontrol.Margin = new Padding(0, 4, 0, 4);
            form.Controls.Add(kontrol, 1, satir);
        }

        public static void DaireCombo(ComboBox combo, bool bosSecenek)
        {
            combo.Items.Clear();
            if (bosSecenek)
            {
                combo.Items.Add(new ListeKalemi(0, "(Daire seçilmedi)"));
            }

            foreach (ListeKalemi kalem in Veritabani.DaireListesi())
            {
                combo.Items.Add(kalem);
            }

            if (combo.Items.Count > 0)
            {
                combo.SelectedIndex = 0;
            }
        }

        public static int? DaireId(ComboBox combo)
        {
            if (combo.SelectedItem is not ListeKalemi kalem || kalem.Id <= 0)
            {
                return null;
            }

            return kalem.Id;
        }

        public static string Plaka(string ham)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (char c in ham.Trim().ToUpperInvariant())
            {
                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public static bool PlakaGecerli(string plaka) => plaka.Length is >= 5 and <= 15;

        public static void Yaz(int kullaniciId, string tur, string aciklama)
        {
            if (aciklama.Length > 500)
            {
                aciklama = aciklama[..500];
            }

            Veritabani.Calistir(
                @"INSERT INTO GuvenlikIslemKayitlari (KullaniciID, IslemTuru, Aciklama)
                  VALUES (@id, @tur, @aciklama)",
                ("@id", kullaniciId),
                ("@tur", tur),
                ("@aciklama", aciklama));
        }

        public static int? SeciliId(DataGridView grid, string kolon)
        {
            if (grid.CurrentRow == null || grid.CurrentRow.IsNewRow || !grid.Columns.Contains(kolon))
            {
                return null;
            }

            object? deger = grid.CurrentRow.Cells[kolon].Value;
            if (deger == null || deger == DBNull.Value)
            {
                return null;
            }

            return Convert.ToInt32(deger);
        }
    }
}
