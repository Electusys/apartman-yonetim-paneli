using System.Security.Cryptography;

namespace ApartmanMobil;

/// <summary>
/// Masaüstü panel ile aynı PBKDF2 biçimi: pbkdf2$tur$tuz$ozet
/// </summary>
public static class SifreKoruma
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
        byte[] ozet = Rfc2898DeriveBytes.Pbkdf2(sifre, tuz, Tur, HashAlgorithmName.SHA256, OzetUzunluk);
        return Onek + Tur + "$" + Convert.ToBase64String(tuz) + "$" + Convert.ToBase64String(ozet);
    }

    public static bool Dogrula(string sifre, string kayit)
    {
        if (!OzetMi(kayit))
        {
            return false;
        }

        string[] parca = kayit.Split('$');
        if (parca.Length != 4 || !int.TryParse(parca[1], out int tur) || tur < 1)
        {
            return false;
        }

        try
        {
            byte[] tuz = Convert.FromBase64String(parca[2]);
            byte[] beklenen = Convert.FromBase64String(parca[3]);
            byte[] gelen = Rfc2898DeriveBytes.Pbkdf2(sifre, tuz, tur, HashAlgorithmName.SHA256, beklenen.Length);
            return CryptographicOperations.FixedTimeEquals(gelen, beklenen);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
