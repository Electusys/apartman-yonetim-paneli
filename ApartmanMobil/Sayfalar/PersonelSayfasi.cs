namespace ApartmanMobil;

public sealed class PersonelSayfasi : TemelSayfa
{
    private readonly Entry _ad = Arayuz.Kutu("Ad");
    private readonly Entry _soyad = Arayuz.Kutu("Soyad");
    private readonly Entry _kullanici = Arayuz.Kutu("Kullanıcı adı");
    private readonly Entry _sifre = Arayuz.Kutu("Şifre", sifre: true);
    private readonly Entry _tekrar = Arayuz.Kutu("Şifre tekrar", sifre: true);
    private readonly Picker _rol;
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };

    public PersonelSayfasi() : base("Personel")
    {
        _rol = Arayuz.Secim("Rol", new List<string> { Veritabani.RolMuhasebeci, Veritabani.RolTeknisyen, Veritabani.RolGuvenlik });
        Button olustur = Arayuz.Birincil("Personel oluştur");
        olustur.Clicked += async (_, _) => await Guvenli(Olustur, "Personel oluşturulurken bir hata oluştu");

        Kok.Add(Arayuz.Yazi("Muhasebeci, teknisyen veya güvenlik hesabı oluşturabilirsiniz.", soluk: true, boyut: 13));
        Kok.Add(Arayuz.Alan("Ad", _ad));
        Kok.Add(Arayuz.Alan("Soyad", _soyad));
        Kok.Add(Arayuz.Alan("Kullanıcı adı", _kullanici));
        Kok.Add(Arayuz.Alan("Şifre", _sifre));
        Kok.Add(Arayuz.Alan("Şifre tekrar", _tekrar));
        Kok.Add(Arayuz.Alan("Rol", _rol));
        Kok.Add(olustur);
        Kok.Add(Arayuz.Baslik("PERSONEL"));
        Kok.Add(_liste);
    }

    protected override async Task Yukle()
    {
        List<Satir> kayitlar = await Task.Run(Listeler.Personeller);
        Arayuz.Listele(_liste, kayitlar.Select(AdminSayfasi.KullaniciKarti).ToList(), "Henüz personel yok.");
    }

    private async Task Olustur()
    {
        string rol = _rol.SelectedItem?.ToString() ?? "";
        Sonuc sonuc = await Task.Run(() => Islemler.PersonelOlustur(_ad.Text, _soyad.Text, _kullanici.Text, _sifre.Text, _tekrar.Text, rol));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        _ad.Text = _soyad.Text = _kullanici.Text = _sifre.Text = _tekrar.Text = "";
        _rol.SelectedIndex = 0;
        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }
}
