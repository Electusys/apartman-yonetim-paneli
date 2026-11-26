using System.Security.Cryptography;

namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Şifreyi PBKDF2 özeti olarak saklar. Düz metin tabloya yazılmaz.
    /// Biçim: pbkdf2$tur$tuz$ozet
    /// </summary>
    internal static class SifreKoruma
    {
        public const string Onek = "pbkdf2$";
        private const int Tur = 100_000;
        private const int TuzUzunluk = 16;
        private const int OzetUzunluk = 32;

        public static bool OzetMi(string? kayit)
        {
            return kayit != null && kayit.StartsWith(Onek, StringComparison.Ordinal);
        }

        public static string Ozetle(string sifre)
        {
            byte[] tuz = RandomNumberGenerator.GetBytes(TuzUzunluk);
            byte[] ozet = Turev(sifre, tuz, Tur, OzetUzunluk);
            return Birlestir(Tur, tuz, ozet);
        }

        public static bool Dogrula(string sifre, string kayit)
        {
            if (!Ayir(kayit, out int tur, out byte[] tuz, out byte[] beklenen))
            {
                return false;
            }

            byte[] gelen = Turev(sifre, tuz, tur, beklenen.Length);
            return CryptographicOperations.FixedTimeEquals(gelen, beklenen);
        }

        private static string Birlestir(int tur, byte[] tuz, byte[] ozet)
        {
            return Onek + tur + "$" + Convert.ToBase64String(tuz) + "$" + Convert.ToBase64String(ozet);
        }

        private static bool Ayir(string kayit, out int tur, out byte[] tuz, out byte[] ozet)
        {
            tur = 0;
            tuz = Array.Empty<byte>();
            ozet = Array.Empty<byte>();
            if (!OzetMi(kayit))
            {
                return false;
            }

            string[] parca = kayit.Split('$');
            if (parca.Length != 4 || !int.TryParse(parca[1], out tur) || tur < 1)
            {
                return false;
            }

            try
            {
                tuz = Convert.FromBase64String(parca[2]);
                ozet = Convert.FromBase64String(parca[3]);
                return tuz.Length > 0 && ozet.Length > 0;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static byte[] Turev(string sifre, byte[] tuz, int tur, int uzunluk)
        {
            return Rfc2898DeriveBytes.Pbkdf2(sifre, tuz, tur, HashAlgorithmName.SHA256, uzunluk);
        }
    }
}
