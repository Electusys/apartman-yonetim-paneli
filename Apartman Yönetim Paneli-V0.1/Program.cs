namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Uygulamanın giriş noktası.
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new OturumBaglami());
        }
    }

    /// <summary>
    /// Giriş ve panel aynı anda kapanmasın diye pencereleri sırayla gösterir.
    /// ShowDialog kullanılmaz: Windows diyalog penceresini görev çubuğundan düşürüp arkaya atıyor.
    /// </summary>
    internal sealed class OturumBaglami : ApplicationContext
    {
        public static OturumBaglami Aktif { get; private set; } = null!;

        public OturumBaglami()
        {
            Aktif = this;
            GirisAc();
        }

        public void OturumuAc(FrmGiris giris, OturumBilgisi oturum)
        {
            Form panel = PanelOlustur(oturum);
            panel.ShowInTaskbar = true;
            panel.StartPosition = FormStartPosition.CenterScreen;
            panel.FormClosed += (_, _) =>
            {
                panel.Dispose();
                GirisAc();
            };

            // Panel görünür ve öndeyken giriş kapanır; arada penceresiz an kalmaz.
            panel.Show();
            OneGetir(panel);
            giris.Close();
        }

        private void GirisAc()
        {
            FrmGiris giris = new FrmGiris();
            giris.FormClosed += (_, _) =>
            {
                bool baskaPencereVar = false;
                foreach (Form acik in Application.OpenForms)
                {
                    if (!ReferenceEquals(acik, giris))
                    {
                        baskaPencereVar = true;
                        break;
                    }
                }

                giris.Dispose();
                if (!baskaPencereVar)
                {
                    ExitThread();
                }
            };
            giris.Show();
            OneGetir(giris);
        }

        private static Form PanelOlustur(OturumBilgisi oturum)
        {
            return string.Equals(oturum.RolAdi, Veritabani.RolAdmin, StringComparison.OrdinalIgnoreCase)
                ? new FrmAdmin(oturum.AdSoyad)
                : new FrmAnaPanel(oturum.KullaniciId, oturum.AdSoyad, oturum.RolAdi);
        }

        private static void OneGetir(Form form)
        {
            if (form.WindowState == FormWindowState.Minimized)
            {
                form.WindowState = FormWindowState.Normal;
            }

            form.TopMost = true;
            form.Activate();
            form.BringToFront();
            form.TopMost = false;
        }
    }
}
