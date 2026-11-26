namespace ApartmanMobil;

public sealed class AidatSayfasi : TemelSayfa
{
    private readonly Oturum _oturum;
    private readonly DatePicker _donem = Arayuz.Tarih();
    private readonly Entry _tutar = Arayuz.Kutu("0", klavye: Keyboard.Numeric);
    private readonly DatePicker _son = Arayuz.Tarih(DateTime.Today.AddDays(15));
    private readonly VerticalStackLayout _daireler = new VerticalStackLayout { Spacing = 4 };
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };
    private readonly List<(ListeKalemi Kalem, CheckBox Kutu)> _secim = new List<(ListeKalemi, CheckBox)>();

    public AidatSayfasi(Oturum oturum) : base("Aidat")
    {
        _oturum = oturum;
        Button tumu = Arayuz.Ikincil("Tüm daireleri seç");
        tumu.Clicked += (_, _) =>
        {
            foreach ((ListeKalemi _, CheckBox kutu) in _secim)
            {
                kutu.IsChecked = true;
            }
        };
        Button olustur = Arayuz.Birincil("Yeni aidat oluştur");
        olustur.Clicked += async (_, _) => await Guvenli(Olustur, "Aidat oluşturulamadı");

        Kok.Add(Arayuz.Yazi("Dönem ve tutarı girip aidat uygulanacak daireleri seçin.", soluk: true, boyut: 13));
        Kok.Add(Arayuz.Alan("Aidat dönemi (ay/yıl)", _donem));
        Kok.Add(Arayuz.Alan("Tutar", _tutar));
        Kok.Add(Arayuz.Alan("Son ödeme tarihi", _son));
        Kok.Add(Arayuz.Baslik("UYGULANACAK DAİRELER"));
        Kok.Add(_daireler);
        Kok.Add(tumu);
        Kok.Add(olustur);
        Kok.Add(Arayuz.Baslik("BORÇLAR"));
        Kok.Add(_liste);
    }

    protected override async Task Yukle()
    {
        (List<ListeKalemi> daireler, List<Satir> borclar) = await Task.Run(() => (Veritabani.DaireListesi(), Listeler.AidatBorclari()));
        _secim.Clear();
        _daireler.Children.Clear();
        foreach (ListeKalemi kalem in daireler)
        {
            CheckBox kutu = new CheckBox { IsChecked = true, Color = Tema.Vurgu };
            _secim.Add((kalem, kutu));
            _daireler.Add(new HorizontalStackLayout
            {
                Spacing = 8,
                Children =
                {
                    kutu,
                    new Label { Text = kalem.Metin, TextColor = Tema.Yazi, VerticalOptions = LayoutOptions.Center }
                }
            });
        }

        if (daireler.Count == 0)
        {
            _daireler.Add(Arayuz.Yazi("Önce blok ve daire kaydı oluşturun.", soluk: true, boyut: 13));
        }

        Arayuz.Listele(_liste, borclar.Select(s => Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Arayuz.Yazi(s.Metin("Daire") + " · " + s.Metin("Donem"), kalin: true),
                Arayuz.Yazi("Tutar " + s.Metin("Tutar") + " ₺  ·  Kalan " + s.Metin("KalanTutar") + " ₺", soluk: true, boyut: 13),
                Arayuz.Yazi(s.Metin("Durum") + " · Son ödeme " + s.Metin("SonOdemeTarihi"), soluk: true, boyut: 12)
            }
        })).ToList(), "Henüz aidat borcu yok.");
    }

    private async Task Olustur()
    {
        if (!Arayuz.Tutar(_tutar.Text, out decimal tutar))
        {
            await Uyari("Tutar geçersiz. Sıfırdan büyük bir tutar giriniz.");
            return;
        }

        List<int> daireler = _secim.Where(x => x.Kutu.IsChecked).Select(x => x.Kalem.Id).ToList();
        DateTime donemGun = _donem.Date ?? DateTime.Today;
        string donem = donemGun.ToString("yyyy-MM");
        DateTime son = _son.Date ?? DateTime.Today;
        Sonuc sonuc = await Task.Run(() => Islemler.AidatOlustur(_oturum.Id, donem, tutar, son, daireler));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }
}
