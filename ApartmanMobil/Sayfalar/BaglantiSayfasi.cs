namespace ApartmanMobil;

public sealed class BaglantiSayfasi : TemelSayfa
{
    private readonly Entry _sunucu = Arayuz.Kutu("127.0.0.1");
    private readonly Entry _port = Arayuz.Kutu("3306", klavye: Keyboard.Numeric);
    private readonly Entry _veritabani = Arayuz.Kutu("ApartmanYonetimDB");
    private readonly Entry _kullanici = Arayuz.Kutu("root");
    private readonly Entry _sifre = Arayuz.Kutu("MySQL şifresi", sifre: true);

    public BaglantiSayfasi() : base("Bağlantı")
    {
        _sunucu.Text = BaglantiAyar.Sunucu;
        _port.Text = BaglantiAyar.Port;
        _veritabani.Text = BaglantiAyar.VeritabaniAdi;
        _kullanici.Text = BaglantiAyar.Kullanici;
        _sifre.Text = BaglantiAyar.Sifre;

        Button wifi = Arayuz.Ikincil("Wi-Fi adresi: " + BaglantiAyar.OnerilenAgAdresi);
        wifi.Clicked += (_, _) => _sunucu.Text = BaglantiAyar.OnerilenAgAdresi;
        Button yerel = Arayuz.Ikincil("USB adresi: 127.0.0.1");
        yerel.Clicked += (_, _) => _sunucu.Text = "127.0.0.1";
        Button dene = Arayuz.Birincil("Kaydet ve dene");
        dene.Clicked += async (_, _) => await Guvenli(Dene, "Bağlantı denemesi başarısız");

        Kok.Add(Arayuz.Yazi("Telefon, bilgisayardaki MySQL veritabanına bağlanır.", soluk: true, boyut: 13));
        Kok.Add(Arayuz.Yazi("Telefon bilgisayarla aynı Wi-Fi ağında olmalı. Dosya aktarımı veritabanını açmaz. 127.0.0.1 yalnızca USB hata ayıklama izni verildiyse çalışır.", soluk: true, boyut: 13));
        Kok.Add(Arayuz.Alan("Sunucu", _sunucu));
        Kok.Add(Arayuz.Alan("Port", _port));
        Kok.Add(Arayuz.Alan("Veritabanı", _veritabani));
        Kok.Add(Arayuz.Alan("Kullanıcı", _kullanici));
        Kok.Add(Arayuz.Alan("Şifre", _sifre));
        Kok.Add(yerel);
        Kok.Add(wifi);
        Kok.Add(dene);
    }

    private async Task Dene()
    {
        if (string.IsNullOrWhiteSpace(_sunucu.Text) || string.IsNullOrWhiteSpace(_veritabani.Text) || string.IsNullOrWhiteSpace(_kullanici.Text))
        {
            await Uyari("Sunucu, veritabanı ve kullanıcı boş bırakılamaz.");
            return;
        }

        BaglantiAyar.Kaydet(_sunucu.Text, _port.Text, _veritabani.Text, _kullanici.Text, _sifre.Text ?? "");
        string mesaj = await Task.Run(Islemler.BaglantiDene);
        await Bilgi(mesaj);
    }
}
