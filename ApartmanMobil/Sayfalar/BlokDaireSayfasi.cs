namespace ApartmanMobil;

public sealed class BlokDaireSayfasi : TemelSayfa
{
    private readonly Entry _blokAdi = Arayuz.Kutu("Blok adı");
    private readonly Entry _katSayisi = Arayuz.Kutu("Kat sayısı", klavye: Keyboard.Numeric);
    private readonly Entry _aciklama = Arayuz.Kutu("Açıklama");
    private readonly Picker _blok = Arayuz.Secim("Blok", new List<string>());
    private readonly Entry _daireNo = Arayuz.Kutu("Daire no");
    private readonly Entry _kat = Arayuz.Kutu("Kat", klavye: Keyboard.Numeric);
    private readonly VerticalStackLayout _bloklar = new VerticalStackLayout { Spacing = 8 };
    private readonly VerticalStackLayout _daireler = new VerticalStackLayout { Spacing = 8 };

    public BlokDaireSayfasi() : base("Blok ve Daire")
    {
        Button blok = Arayuz.Birincil("Blok kaydet");
        blok.Clicked += async (_, _) => await Guvenli(BlokKaydet, "Blok eklenirken hata oluştu");
        Button daire = Arayuz.Birincil("Daire kaydet");
        daire.Clicked += async (_, _) => await Guvenli(DaireKaydet, "Daire eklenirken hata oluştu");

        Kok.Add(Arayuz.Baslik("YENİ BLOK"));
        Kok.Add(Arayuz.Alan("Blok adı", _blokAdi));
        Kok.Add(Arayuz.Alan("Kat sayısı", _katSayisi));
        Kok.Add(Arayuz.Alan("Açıklama", _aciklama));
        Kok.Add(blok);
        Kok.Add(Arayuz.Baslik("YENİ DAİRE"));
        Kok.Add(Arayuz.Alan("Blok", _blok));
        Kok.Add(Arayuz.Alan("Daire no", _daireNo));
        Kok.Add(Arayuz.Alan("Kat", _kat));
        Kok.Add(daire);
        Kok.Add(Arayuz.Baslik("BLOKLAR"));
        Kok.Add(_bloklar);
        Kok.Add(Arayuz.Baslik("DAİRELER"));
        Kok.Add(_daireler);
    }

    protected override async Task Yukle()
    {
        (List<Satir> bloklar, List<Satir> daireler, List<string> adlar) = await Task.Run(() =>
            (Listeler.Bloklar(), Listeler.Daireler(), Listeler.BlokAdlari()));

        string? secili = _blok.SelectedItem?.ToString();
        _blok.Items.Clear();
        foreach (string ad in adlar)
        {
            _blok.Items.Add(ad);
        }

        if (secili is not null && _blok.Items.Contains(secili))
        {
            _blok.SelectedItem = secili;
        }
        else if (_blok.Items.Count > 0)
        {
            _blok.SelectedIndex = 0;
        }

        Arayuz.Listele(_bloklar, bloklar.Select(s => Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Arayuz.Yazi(s.Metin("BlokAdi"), kalin: true),
                Arayuz.Yazi("Kat sayısı: " + s.Metin("KatSayisi") + (string.IsNullOrWhiteSpace(s.Metin("Aciklama")) ? "" : " · " + s.Metin("Aciklama")), soluk: true, boyut: 13)
            }
        })).ToList(), "Henüz blok yok.");

        Arayuz.Listele(_daireler, daireler.Select(s => Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Arayuz.Yazi(s.Metin("Blok") + " / " + s.Metin("DaireNo"), kalin: true),
                Arayuz.Yazi("Kat: " + s.Metin("Kat"), soluk: true, boyut: 13)
            }
        })).ToList(), "Henüz daire yok.");
    }

    private async Task BlokKaydet()
    {
        if (!int.TryParse(string.IsNullOrWhiteSpace(_katSayisi.Text) ? "0" : _katSayisi.Text.Trim(), out int kat))
        {
            await Uyari("Kat sayısı sayı olmalı.");
            return;
        }

        Sonuc sonuc = await Task.Run(() => Islemler.BlokEkle(_blokAdi.Text, kat, _aciklama.Text));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        _blokAdi.Text = _aciklama.Text = "";
        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }

    private async Task DaireKaydet()
    {
        if (!int.TryParse(string.IsNullOrWhiteSpace(_kat.Text) ? "0" : _kat.Text.Trim(), out int kat))
        {
            await Uyari("Kat sayı olmalı.");
            return;
        }

        Sonuc sonuc = await Task.Run(() => Islemler.DaireEkle(_blok.SelectedItem?.ToString(), _daireNo.Text, kat));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        _daireNo.Text = "";
        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }
}
