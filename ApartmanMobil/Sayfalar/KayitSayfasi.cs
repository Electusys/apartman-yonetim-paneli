namespace ApartmanMobil;

public sealed class KayitSayfasi : TemelSayfa
{
    private readonly Entry _ad = Arayuz.Kutu("Ad");
    private readonly Entry _soyad = Arayuz.Kutu("Soyad");
    private readonly Entry _kullanici = Arayuz.Kutu("Kullanıcı adı");
    private readonly Entry _sifre = Arayuz.Kutu("Şifre", sifre: true);
    private readonly Entry _tekrar = Arayuz.Kutu("Şifre tekrar", sifre: true);

    public KayitSayfasi() : base("Kayıt Ol")
    {
        _ad.MaxLength = 50;
        _soyad.MaxLength = 50;
        _kullanici.MaxLength = 50;
        Button kaydet = Arayuz.Birincil("Kayıt Ol");
        kaydet.Clicked += async (_, _) => await Guvenli(Kaydet, "Kayıt sırasında bir hata oluştu");

        Kok.Add(Arayuz.Yazi("Bu ekrandan yalnızca apartman sakini hesabı oluşturulur. Yönetici hesapları Admin panelinden açılır.", soluk: true, boyut: 13));
        Kok.Add(Arayuz.Alan("Ad", _ad));
        Kok.Add(Arayuz.Alan("Soyad", _soyad));
        Kok.Add(Arayuz.Alan("Kullanıcı adı", _kullanici));
        Kok.Add(Arayuz.Alan("Şifre", _sifre));
        Kok.Add(Arayuz.Alan("Şifre tekrar", _tekrar));
        Kok.Add(kaydet);
    }

    private async Task Kaydet()
    {
        Sonuc sonuc = await Task.Run(() => Islemler.SakinKaydi(_ad.Text, _soyad.Text, _kullanici.Text, _sifre.Text, _tekrar.Text));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        await Bilgi(sonuc.Mesaj);
        await Navigation.PopAsync();
    }
}
