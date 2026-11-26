namespace ApartmanMobil;

public sealed class YorumSayfasi : TemelSayfa
{
    private readonly Oturum _oturum;
    private readonly int _duyuruId;
    private readonly Entry _yorum = Arayuz.Kutu("Yorumunuz");
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };

    public YorumSayfasi(Oturum oturum, int duyuruId, string baslik, string icerik) : base("Yorumlar")
    {
        _oturum = oturum;
        _duyuruId = duyuruId;
        Button gonder = Arayuz.Birincil("Yorumu gönder");
        gonder.Clicked += async (_, _) => await Guvenli(Gonder, "Yorum kaydedilemedi");

        Kok.Add(Arayuz.Yazi(baslik, boyut: 22, kalin: true));
        Kok.Add(Arayuz.Yazi(icerik, boyut: 15));
        Kok.Add(Arayuz.Baslik("YORUMLAR"));
        Kok.Add(_liste);
        Kok.Add(Arayuz.Alan("Yorum", _yorum));
        Kok.Add(gonder);
    }

    protected override async Task Yukle()
    {
        int id = _duyuruId;
        List<Satir> kayitlar = await Task.Run(() =>
        {
            Islemler.YorumTablosunuHazirla();
            return Listeler.Yorumlar(id);
        });
        Arayuz.Listele(_liste, kayitlar.Select(s => Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Arayuz.Yazi(s.Metin("Yazan") + " · " + s.Metin("Rol"), kalin: true),
                Arayuz.Yazi(s.Metin("Yorum")),
                Arayuz.Yazi(s.Metin("Tarih"), soluk: true, boyut: 12)
            }
        })).ToList(), "Henüz yorum yok.");
    }

    private async Task Gonder()
    {
        Sonuc sonuc = await Task.Run(() => Islemler.YorumEkle(_duyuruId, _oturum.Id, _yorum.Text));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        _yorum.Text = "";
        await Yukle();
    }
}
