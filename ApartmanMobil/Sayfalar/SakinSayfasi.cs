namespace ApartmanMobil;

public sealed class SakinSayfasi : TemelSayfa
{
    private readonly Entry _ad = Arayuz.Kutu("Ad");
    private readonly Entry _soyad = Arayuz.Kutu("Soyad");
    private readonly Entry _telefon = Arayuz.Kutu("Telefon", klavye: Keyboard.Telephone);
    private readonly Picker _daire = new Picker { Title = "Daire", TextColor = Tema.Yazi, TitleColor = Tema.YaziSoluk, BackgroundColor = Colors.Transparent, HeightRequest = 44 };
    private readonly Picker _hesap = new Picker { Title = "Giriş hesabı", TextColor = Tema.Yazi, TitleColor = Tema.YaziSoluk, BackgroundColor = Colors.Transparent, HeightRequest = 44 };
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };

    public SakinSayfasi() : base("Sakinler")
    {
        Button ekle = Arayuz.Birincil("Yeni sakin ekle");
        ekle.Clicked += async (_, _) => await Guvenli(Ekle, "Sakin kaydı oluşturulamadı");

        Kok.Add(Arayuz.Yazi("Daire seçerek yeni sakin kaydı oluşturun.", soluk: true, boyut: 13));
        Kok.Add(Arayuz.Alan("Ad", _ad));
        Kok.Add(Arayuz.Alan("Soyad", _soyad));
        Kok.Add(Arayuz.Alan("Telefon", _telefon));
        Kok.Add(Arayuz.Alan("Daire", _daire));
        Kok.Add(Arayuz.Alan("Giriş hesabı (isteğe bağlı)", _hesap));
        Kok.Add(ekle);
        Kok.Add(Arayuz.Baslik("SAKİNLER"));
        Kok.Add(_liste);
    }

    protected override async Task Yukle()
    {
        (List<Satir> sakinler, List<ListeKalemi> daireler, List<ListeKalemi> hesaplar) = await Task.Run(() =>
            (Listeler.Sakinler(), Veritabani.DaireListesi(), Listeler.SakinHesaplari()));
        PickerDoldur(_daire, daireler);
        PickerDoldur(_hesap, hesaplar);
        Arayuz.Listele(_liste, sakinler.Select(s => Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Arayuz.Yazi(s.Metin("Ad") + " " + s.Metin("Soyad"), kalin: true),
                Arayuz.Yazi(s.Metin("Daire") + " · " + s.Metin("Durum"), soluk: true, boyut: 13),
                Arayuz.Yazi(string.IsNullOrWhiteSpace(s.Metin("Telefon")) ? s.Metin("KullaniciAdi") : s.Metin("Telefon") + " · " + s.Metin("KullaniciAdi"), soluk: true, boyut: 12)
            }
        })).ToList(), "Henüz sakin yok.");
    }

    private async Task Ekle()
    {
        int daireId = _daire.SelectedItem is ListeKalemi daire ? daire.Id : 0;
        int hesapId = _hesap.SelectedItem is ListeKalemi hesap ? hesap.Id : 0;
        Sonuc sonuc = await Task.Run(() => Islemler.SakinEkle(_ad.Text, _soyad.Text, _telefon.Text, daireId, hesapId));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        _ad.Text = _soyad.Text = _telefon.Text = "";
        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }
}
