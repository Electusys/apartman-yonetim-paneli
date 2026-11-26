namespace ApartmanMobil;

public sealed class AidatlarimSayfasi : TemelSayfa
{
    private readonly Oturum _oturum;
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };

    public AidatlarimSayfasi(Oturum oturum) : base("Aidatlarım")
    {
        _oturum = oturum;
        Kok.Add(Arayuz.Yazi("Dairenize tanımlı aidat borçlarını ve kalan tutarı izleyin.", soluk: true, boyut: 13));
        Kok.Add(_liste);
    }

    protected override async Task Yukle()
    {
        int id = _oturum.Id;
        List<Satir> kayitlar = await Task.Run(() => Listeler.Aidatlarim(id));
        Arayuz.Listele(_liste, kayitlar.Select(s => Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Arayuz.Yazi(s.Metin("Daire") + " · " + s.Metin("Donem"), kalin: true),
                Arayuz.Yazi("Tutar " + s.Metin("Tutar") + " ₺  ·  Kalan " + s.Metin("KalanTutar") + " ₺", soluk: true, boyut: 13),
                Arayuz.Yazi(s.Metin("Durum") + " · Son ödeme " + s.Metin("SonOdemeTarihi"), soluk: true, boyut: 12)
            }
        })).ToList(), "Dairenize bağlı aidat bulunmuyor.");
    }
}
