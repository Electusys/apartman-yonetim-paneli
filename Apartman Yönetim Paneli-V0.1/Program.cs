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
            // Yüksek DPI, varsayılan font ve görsel stilleri ayarlar.
            ApplicationConfiguration.Initialize();

            // Uygulama kapanana kadar giriş formunu çalıştırır.
            // Oturum açılınca bu form gizlenir; çıkışta tekrar gösterilir.
            Application.Run(new FrmGiris());
        }
    }
}
