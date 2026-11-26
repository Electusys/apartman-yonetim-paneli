namespace ApartmanMobil;

public sealed class DuyuruSayfasi : TemelSayfa
{
    private readonly Oturum _oturum;
    private readonly Entry _baslik = Arayuz.Kutu("Başlık");
    private readonly Editor _icerik = Arayuz.MetinAlani("Duyuru metni");
    private readonly DatePicker _gun = Arayuz.Tarih();
    private readonly TimePicker _saat = new TimePicker { Time = DateTime.Now.TimeOfDay, TextColor = Tema.Yazi, BackgroundColor = Colors.Transparent, HeightRequest = 44 };
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };

    public DuyuruSayfasi(Oturum oturum) : base("Duyurular")
    {
        _oturum = oturum;
        _baslik.MaxLength = 120;
        if (oturum.YoneticiMi)
        {
            Button yayimla = Arayuz.Birincil("Duyuruyu yayımla");
            yayimla.Clicked += async (_, _) => await Guvenli(Yayimla, "Duyuru yayımlanamadı");
            Kok.Add(Arayuz.Yazi("Başlık, içerik ve yayımlanma zamanını girin.", soluk: true, boyut: 13));
            Kok.Add(Arayuz.Alan("Başlık", _baslik));
            Kok.Add(Arayuz.Alan("İçerik", _icerik));
            Kok.Add(Arayuz.Alan("Gün", _gun));
            Kok.Add(Arayuz.Alan("Saat", _saat));
            Kok.Add(yayimla);
        }
        else
        {
            Kok.Add(Arayuz.Yazi("Yayımlanmış duyuruları okuyup yorum yazabilirsiniz.", soluk: true, boyut: 13));
        }

        Kok.Add(Arayuz.Baslik("DUYURULAR"));
        Kok.Add(_liste);
    }

    protected override async Task Yukle()
    {
        bool yonetici = _oturum.YoneticiMi;
        List<Satir> kayitlar = await Task.Run(() =>
        {
            Islemler.YorumTablosunuHazirla();
            return Listeler.Duyurular(yonetici);
        });

        Arayuz.Listele(_liste, kayitlar.Select(s =>
        {
            Satir duyuru = s;
            return Arayuz.Kart(new VerticalStackLayout
            {
                Spacing = 4,
                Children =
                {
                    Arayuz.Yazi(duyuru.Metin("Baslik"), kalin: true),
                    Arayuz.Yazi(duyuru.Metin("Icerik"), boyut: 14),
                    Arayuz.Yazi(duyuru.Metin("Yazan").Trim() + " · " + duyuru.Metin("YayimTarihi") + " · " + duyuru.Metin("YorumSayisi") + " yorum", soluk: true, boyut: 12)
                }
            }, () => _ = Navigation.PushAsync(new YorumSayfasi(_oturum, duyuru.Tam("DuyuruID"), duyuru.Metin("Baslik"), duyuru.Metin("Icerik"))));
        }).ToList(), "Henüz duyuru yok.");
    }

    private async Task Yayimla()
    {
        DateTime gun = _gun.Date ?? DateTime.Today;
        TimeSpan saat = _saat.Time ?? TimeSpan.Zero;
        DateTime zaman = gun.Date.Add(saat);
        Sonuc sonuc = await Task.Run(() => Islemler.DuyuruYayimla(_oturum.Id, _baslik.Text, _icerik.Text, zaman));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        _baslik.Text = _icerik.Text = "";
        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }
}
