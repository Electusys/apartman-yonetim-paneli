namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Uygulamanın giriş noktası.
    /// Windows Forms uygulaması burada başlar ve ilk ekran olarak giriş formu açılır.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// STAThread: WinForms kontrolleri tek iş parçacıklı COM modeline ihtiyaç duyar.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Giriş formu gizlenip ikinci form Show() edilince Windows görev çubuğu
            // simgesini düşürüyor ve pencere arkaya gidiyor. Her ekran kendi
            // ShowDialog oturumunda, görev çubuğunda görünür şekilde açılır.
            while (true)
            {
                OturumBilgisi? oturum;
                using (FrmGiris giris = new FrmGiris())
                {
                    giris.ShowInTaskbar = true;
                    if (giris.ShowDialog() != DialogResult.OK || giris.Oturum is null)
                    {
                        return;
                    }

                    oturum = giris.Oturum;
                }

                using Form panel = OturumFormu(oturum);
                panel.ShowInTaskbar = true;
                panel.ShowDialog();
            }
        }

        private static Form OturumFormu(OturumBilgisi oturum)
        {
            return string.Equals(oturum.RolAdi, Veritabani.RolAdmin, StringComparison.OrdinalIgnoreCase)
                ? new FrmAdmin(oturum.AdSoyad)
                : new FrmAnaPanel(oturum.KullaniciId, oturum.AdSoyad, oturum.RolAdi);
        }
    }
}
