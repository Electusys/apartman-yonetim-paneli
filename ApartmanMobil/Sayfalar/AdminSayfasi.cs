namespace ApartmanMobil;

public sealed class AdminSayfasi : TemelSayfa
{
    private readonly Entry _ad = Arayuz.Kutu("Ad");
    private readonly Entry _soyad = Arayuz.Kutu("Soyad");
    private readonly Entry _kullanici = Arayuz.Kutu("Kullanıcı adı");
    private readonly Entry _sifre = Arayuz.Kutu("Şifre", sifre: true);
    private readonly Entry _tekrar = Arayuz.Kutu("Şifre tekrar", sifre: true);
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };

    public AdminSayfasi(Oturum oturum) : base("Yönetici Oluştur")
    {
        _ad.MaxLength = 50;
        _soyad.MaxLength = 50;
        _kullanici.MaxLength = 50;
        Button olustur = Arayuz.Birincil("Yönetici oluştur");
        olustur.Clicked += async (_, _) => await Guvenli(Olustur, "Yönetici oluşturulurken bir hata oluştu");

        Kok.Add(Arayuz.Yazi(oturum.AdSoyad, kalin: true));
        Kok.Add(Arayuz.Yazi("Yönetici rolünü yalnızca Admin atar. Muhasebeci ve teknisyen ataması yönetici panelinden yapılır.", soluk: true, boyut: 13));
        Kok.Add(Arayuz.Alan("Ad", _ad));
        Kok.Add(Arayuz.Alan("Soyad", _soyad));
        Kok.Add(Arayuz.Alan("Kullanıcı adı", _kullanici));
        Kok.Add(Arayuz.Alan("Şifre", _sifre));
        Kok.Add(Arayuz.Alan("Şifre tekrar", _tekrar));
        Kok.Add(olustur);
        Kok.Add(Arayuz.Baslik("YÖNETİCİLER"));
        Kok.Add(_liste);
    }

    protected override async Task Yukle()
    {
        List<Satir> kayitlar = await Task.Run(Listeler.Yoneticiler);
        Arayuz.Listele(_liste, kayitlar.Select(KullaniciKarti).ToList(), "Henüz apartman yöneticisi yok.");
    }

    private async Task Olustur()
    {
        Sonuc sonuc = await Task.Run(() => Islemler.YoneticiOlustur(_ad.Text, _soyad.Text, _kullanici.Text, _sifre.Text, _tekrar.Text));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        _ad.Text = _soyad.Text = _kullanici.Text = _sifre.Text = _tekrar.Text = "";
        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }

    internal static View KullaniciKarti(Satir satir)
    {
        return Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Arayuz.Yazi(satir.Metin("Ad") + " " + satir.Metin("Soyad"), kalin: true),
                Arayuz.Yazi(satir.Metin("KullaniciAdi") + " · " + satir.Metin("RolAdi"), soluk: true, boyut: 13)
            }
        });
    }
}
