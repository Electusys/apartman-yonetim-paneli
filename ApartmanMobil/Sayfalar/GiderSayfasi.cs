namespace ApartmanMobil;

public sealed class GiderSayfasi : TemelSayfa
{
    private readonly Oturum _oturum;
    private readonly Picker _kategori = Arayuz.Secim("Kategori", new List<string> { "Elektrik", "Su", "Doğalgaz", "Asansör", "Temizlik", "Bakım", "Diğer" });
    private readonly Entry _aciklama = Arayuz.Kutu("Açıklama");
    private readonly Entry _tutar = Arayuz.Kutu("0", klavye: Keyboard.Numeric);
    private readonly DatePicker _tarih = Arayuz.Tarih();
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };

    public GiderSayfasi(Oturum oturum) : base("Giderler")
    {
        _oturum = oturum;
        Button kaydet = Arayuz.Birincil("Kaydet");
        kaydet.Clicked += async (_, _) => await Guvenli(Kaydet, "Gider kaydedilemedi");

        Kok.Add(Arayuz.Yazi("Kategori, açıklama, tutar ve tarih ile ortak gider ekleyin.", soluk: true, boyut: 13));
        Kok.Add(Arayuz.Alan("Kategori", _kategori));
        Kok.Add(Arayuz.Alan("Açıklama", _aciklama));
        Kok.Add(Arayuz.Alan("Tutar", _tutar));
        Kok.Add(Arayuz.Alan("Tarih", _tarih));
        Kok.Add(kaydet);
        Kok.Add(Arayuz.Baslik("GİDERLER"));
        Kok.Add(_liste);
    }

    protected override async Task Yukle()
    {
        List<Satir> kayitlar = await Task.Run(Listeler.Giderler);
        Arayuz.Listele(_liste, kayitlar.Select(s => Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Arayuz.Yazi(s.Metin("Kategori") + " · " + s.Metin("Tutar") + " ₺", kalin: true),
                Arayuz.Yazi(s.Metin("Aciklama"), soluk: true, boyut: 13),
                Arayuz.Yazi(s.Metin("Tarih"), soluk: true, boyut: 12)
            }
        })).ToList(), "Henüz gider yok.");
    }

    private async Task Kaydet()
    {
        if (!Arayuz.Tutar(_tutar.Text, out decimal tutar))
        {
            await Uyari("Tutar geçersiz veya zorunlu bilgiler eksik.");
            return;
        }

        string? kategori = _kategori.SelectedItem?.ToString();
        DateTime tarih = _tarih.Date ?? DateTime.Today;
        Sonuc sonuc = await Task.Run(() => Islemler.GiderEkle(_oturum.Id, kategori, _aciklama.Text, tutar, tarih));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        _aciklama.Text = "";
        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }
}
