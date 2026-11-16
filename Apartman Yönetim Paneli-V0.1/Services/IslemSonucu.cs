namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// Servis katmanından forma dönen başarı/uyarı sonucu.
    /// </summary>
    internal sealed class IslemSonucu
    {
        public bool Basarili { get; }
        public string Mesaj { get; }

        private IslemSonucu(bool basarili, string mesaj)
        {
            Basarili = basarili;
            Mesaj = mesaj;
        }

        public static IslemSonucu Ok(string mesaj = "") => new(true, mesaj);

        public static IslemSonucu Hata(string mesaj) => new(false, mesaj);
    }

    /// <summary>
    /// Değer taşıyan işlem sonucu (ör. giriş oturumu).
    /// </summary>
    internal sealed class IslemSonucu<T>
    {
        public bool Basarili { get; }
        public string Mesaj { get; }
        public T? Deger { get; }

        private IslemSonucu(bool basarili, string mesaj, T? deger)
        {
            Basarili = basarili;
            Mesaj = mesaj;
            Deger = deger;
        }

        public static IslemSonucu<T> Ok(T deger, string mesaj = "") => new(true, mesaj, deger);

        public static IslemSonucu<T> Hata(string mesaj) => new(false, mesaj, default);
    }
}
