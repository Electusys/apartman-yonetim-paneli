using System.Drawing.Drawing2D;
using System.Reflection;

namespace Apartman_Yönetim_Paneli_V0._1
{
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
                // Görsel yoksa yedek: fıstık yeşili yuvarlak + ev silüeti
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
