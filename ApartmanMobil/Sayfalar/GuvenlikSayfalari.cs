namespace ApartmanMobil;

public sealed class ZiyaretSayfasi : TemelSayfa
{
    private readonly Oturum _oturum;
    private readonly Picker _tur = Arayuz.Secim("Tür", new List<string> { "Misafir", "Kurye", "Araç" });
    private readonly Entry _ad = Arayuz.Kutu("Ad soyad");
    private readonly Picker _daire = new Picker { Title = "Daire", TextColor = Tema.Yazi };
    private readonly Entry _plaka = Arayuz.Kutu("Plaka");
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };

    public ZiyaretSayfasi(Oturum oturum) : base("Ziyaret")
    {
        _oturum = oturum;
        _plaka.IsEnabled = false;
        _tur.SelectedIndexChanged += (_, _) => _plaka.IsEnabled = AracMi();
        Button kaydet = Arayuz.Birincil("Girişi kaydet");
        kaydet.Clicked += async (_, _) => await Guvenli(Kaydet, "Giriş kaydı oluşturulamadı");
        Kok.Add(Arayuz.Yazi("Misafir, kurye veya araç girişi. Çıkış saati ayrılınca yazılır.", soluk: true, boyut: 13));
        Kok.Add(Arayuz.Alan("Tür", _tur));
        Kok.Add(Arayuz.Alan("Ad soyad", _ad));
        Kok.Add(Arayuz.Alan("Daire", _daire));
        Kok.Add(Arayuz.Alan("Plaka", _plaka));
        Kok.Add(kaydet);
        Kok.Add(_liste);
    }

    private bool AracMi() => string.Equals(_tur.SelectedItem?.ToString(), "Araç", StringComparison.Ordinal);

    protected override async Task Yukle()
    {
        List<ListeKalemi> daireler = await Task.Run(Veritabani.DaireListesi);
        List<Satir> kayitlar = await Task.Run(GuvenlikVeri.Ziyaretler);
        PickerDoldur(_daire, daireler);
        List<View> kartlar = new List<View>();
        foreach (Satir satir in kayitlar)
        {
            VerticalStackLayout icerik = new VerticalStackLayout
            {
                Spacing = 4,
                Children =
                {
                    Arayuz.Yazi(satir.Metin("Tur") + " · " + satir.Metin("AdSoyad"), kalin: true),
                    Arayuz.Yazi(satir.Metin("Daire") + "  " + satir.Metin("Plaka"), soluk: true, boyut: 13),
                    Arayuz.Yazi("Giriş " + satir.Metin("Giris") + (string.IsNullOrWhiteSpace(satir.Metin("Cikis")) ? "" : " · Çıkış " + satir.Metin("Cikis")), soluk: true, boyut: 12)
                }
            };
            if (string.IsNullOrWhiteSpace(satir.Metin("Cikis")))
            {
                int id = satir.Tam("ZiyaretID");
                Button cikis = Arayuz.Ikincil("Çıkış yaz");
                cikis.Clicked += async (_, _) => await Guvenli(() => Cikis(id), "Çıkış saati yazılamadı");
                icerik.Add(cikis);
            }

            kartlar.Add(Arayuz.Kart(icerik));
        }

        Arayuz.Listele(_liste, kartlar, "Henüz ziyaret yok.");
    }

    private async Task Kaydet()
    {
        string? plaka = null;
        string tur = _tur.SelectedItem?.ToString() ?? "Misafir";
        if (AracMi())
        {
            plaka = GuvenlikVeri.Plaka(_plaka.Text ?? "");
            if (!GuvenlikVeri.PlakaGecerli(plaka))
            {
                await Uyari("Araç girişinde geçerli bir plaka zorunludur.");
                return;
            }

            tur = "Arac";
        }

        int daireId = _daire.SelectedItem is ListeKalemi daire ? daire.Id : 0;
        Sonuc sonuc = await Task.Run(() => GuvenlikVeri.ZiyaretEkle(_oturum.Id, tur, _ad.Text ?? "", daireId, plaka));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        _ad.Text = "";
        _plaka.Text = "";
        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }

    private async Task Cikis(int id)
    {
        Sonuc sonuc = await Task.Run(() => GuvenlikVeri.ZiyaretCikis(_oturum.Id, id));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }
}

public sealed class KargoSayfasi : TemelSayfa
{
    private readonly Oturum _oturum;
    private readonly Picker _daire = new Picker { Title = "Daire", TextColor = Tema.Yazi };
    private readonly Entry _bilgi = Arayuz.Kutu("Kargo bilgisi");
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };

    public KargoSayfasi(Oturum oturum) : base("Kargo")
    {
        _oturum = oturum;
        Button kaydet = Arayuz.Birincil("Kaydet ve daireye bildir");
        kaydet.Clicked += async (_, _) => await Guvenli(Kaydet, "Kargo kaydı oluşturulamadı");
        Kok.Add(Arayuz.Alan("Daire", _daire));
        Kok.Add(Arayuz.Alan("Kargo bilgisi", _bilgi));
        Kok.Add(kaydet);
        Kok.Add(_liste);
    }

    protected override async Task Yukle()
    {
        (List<ListeKalemi> daireler, List<Satir> kayitlar) = await Task.Run(() => (Veritabani.DaireListesi(), GuvenlikVeri.Kargolar()));
        PickerDoldur(_daire, daireler);
        Arayuz.Listele(_liste, kayitlar.Select(s => (View)Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Arayuz.Yazi(s.Metin("Daire"), kalin: true),
                Arayuz.Yazi(s.Metin("KargoBilgisi"), soluk: true, boyut: 13),
                Arayuz.Yazi(s.Metin("Teslim"), soluk: true, boyut: 12)
            }
        })).ToList(), "Henüz kargo yok.");
    }

    private async Task Kaydet()
    {
        int daireId = _daire.SelectedItem is ListeKalemi daire ? daire.Id : 0;
        Sonuc sonuc = await Task.Run(() => GuvenlikVeri.KargoEkle(_oturum.Id, daireId, _bilgi.Text ?? ""));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        _bilgi.Text = "";
        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }
}

public sealed class RehberSayfasi : TemelSayfa
{
    private readonly Oturum _oturum;
    private readonly Entry _ara = Arayuz.Kutu("Ad, soyad veya daire");
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };

    public RehberSayfasi(Oturum oturum) : base("Sakin rehberi")
    {
        _oturum = oturum;
        Button listele = Arayuz.Birincil("Listele");
        listele.Clicked += async (_, _) => await Guvenli(Yukle, "Sakin rehberi yüklenemedi");
        Kok.Add(Arayuz.Yazi("Salt okunur. Yalnızca ad, soyad, daire ve acil iletişim. Aidat ve kimlik yok.", soluk: true, boyut: 13));
        Kok.Add(Arayuz.Alan("Ara", _ara));
        Kok.Add(listele);
        Kok.Add(_liste);
    }

    protected override async Task Yukle()
    {
        string aranan = (_ara.Text ?? "").Trim();
        List<Satir> kayitlar = await Task.Run(() => GuvenlikVeri.Rehber(aranan));
        try
        {
            await Task.Run(() => GuvenlikVeri.Yaz(_oturum.Id, "Sakin rehberi", string.IsNullOrEmpty(aranan) ? "Rehber listelendi." : "Rehberde arama yapıldı."));
        }
        catch
        {
            // Rehber görünsün; günlük tablosu yoksa liste yine de açılır.
        }

        Arayuz.Listele(_liste, kayitlar.Select(s => (View)Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Arayuz.Yazi(s.Metin("Ad") + " " + s.Metin("Soyad"), kalin: true),
                Arayuz.Yazi(s.Metin("Daire"), soluk: true, boyut: 13),
                Arayuz.Yazi(string.IsNullOrWhiteSpace(s.Metin("AcilIletisim")) ? "Acil iletişim yok" : s.Metin("AcilIletisim"), soluk: true, boyut: 12)
            }
        })).ToList(), "Kayıt yok.");
    }
}

public sealed class PlakaSayfasi : TemelSayfa
{
    private readonly Oturum _oturum;
    private readonly Entry _sorgu = Arayuz.Kutu("Plaka");
    private readonly Label _sonuc;
    private readonly Entry _plaka = Arayuz.Kutu("İzin plakası");
    private readonly Entry _ad = Arayuz.Kutu("Ziyaretçi ad soyad");
    private readonly Picker _daire = new Picker { Title = "Daire", TextColor = Tema.Yazi };
    private readonly DatePicker _basGun = Arayuz.Tarih();
    private readonly TimePicker _basSaat = new TimePicker { Time = DateTime.Now.TimeOfDay, TextColor = Tema.Yazi };
    private readonly DatePicker _bitGun = Arayuz.Tarih(DateTime.Today);
    private readonly TimePicker _bitSaat = new TimePicker { Time = DateTime.Now.AddHours(2).TimeOfDay, TextColor = Tema.Yazi };
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };

    public PlakaSayfasi(Oturum oturum) : base("Plaka ve park")
    {
        _oturum = oturum;
        _sonuc = Arayuz.Yazi("Sorgu sonucu burada görünür.", soluk: true, boyut: 13);
        Button sorgula = Arayuz.Birincil("Plaka sorgula");
        sorgula.Clicked += async (_, _) => await Guvenli(Sorgula, "Plaka sorgulanamadı");
        Button izin = Arayuz.Birincil("Geçici izin oluştur");
        izin.Clicked += async (_, _) => await Guvenli(Izin, "Park izni oluşturulamadı");
        Kok.Add(Arayuz.Alan("Plaka", _sorgu));
        Kok.Add(sorgula);
        Kok.Add(_sonuc);
        Kok.Add(Arayuz.Alan("İzin plakası", _plaka));
        Kok.Add(Arayuz.Alan("Ziyaretçi", _ad));
        Kok.Add(Arayuz.Alan("Daire", _daire));
        Kok.Add(Arayuz.Alan("Başlangıç günü", _basGun));
        Kok.Add(Arayuz.Alan("Başlangıç saati", _basSaat));
        Kok.Add(Arayuz.Alan("Bitiş günü", _bitGun));
        Kok.Add(Arayuz.Alan("Bitiş saati", _bitSaat));
        Kok.Add(izin);
        Kok.Add(_liste);
    }

    protected override async Task Yukle()
    {
        (List<ListeKalemi> daireler, List<Satir> izinler) = await Task.Run(() => (Veritabani.DaireListesi(), GuvenlikVeri.ParkIzinleri()));
        PickerDoldur(_daire, daireler);
        Arayuz.Listele(_liste, izinler.Select(s => (View)Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Arayuz.Yazi(s.Metin("Plaka") + " · " + s.Metin("ZiyaretciAdSoyad"), kalin: true),
                Arayuz.Yazi(s.Metin("Daire"), soluk: true, boyut: 13),
                Arayuz.Yazi(s.Metin("Baslangic") + " - " + s.Metin("Bitis"), soluk: true, boyut: 12)
            }
        })).ToList(), "Henüz geçici izin yok.");
    }

    private async Task Sorgula()
    {
        string plaka = GuvenlikVeri.Plaka(_sorgu.Text ?? "");
        if (!GuvenlikVeri.PlakaGecerli(plaka))
        {
            await Uyari("Geçerli bir plaka girin.");
            return;
        }

        _sonuc.Text = await Task.Run(() => GuvenlikVeri.PlakaSorgula(_oturum.Id, plaka));
    }

    private async Task Izin()
    {
        DateTime bas = (_basGun.Date ?? DateTime.Today).Date.Add(_basSaat.Time ?? TimeSpan.Zero);
        DateTime bit = (_bitGun.Date ?? DateTime.Today).Date.Add(_bitSaat.Time ?? TimeSpan.Zero);
        int daireId = _daire.SelectedItem is ListeKalemi daire ? daire.Id : 0;
        Sonuc sonuc = await Task.Run(() => GuvenlikVeri.ParkIzni(_oturum.Id, GuvenlikVeri.Plaka(_plaka.Text ?? ""), _ad.Text ?? "", daireId, bas, bit));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        _plaka.Text = "";
        _ad.Text = "";
        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }
}

public sealed class OlaySayfasi : TemelSayfa
{
    private readonly Oturum _oturum;
    private readonly Picker _tur = Arayuz.Secim("Tür", new List<string> { "Vardiya Teslimi", "Kural İhlali" });
    private readonly Editor _aciklama = Arayuz.MetinAlani("Açıklama");
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };

    public OlaySayfasi(Oturum oturum) : base("Olay raporu")
    {
        _oturum = oturum;
        Button kaydet = Arayuz.Birincil("Raporu kaydet");
        kaydet.Clicked += async (_, _) => await Guvenli(Kaydet, "Olay kaydı oluşturulamadı");
        Kok.Add(Arayuz.Yazi("Saat sistem tarafından basılır. Kayıt sonra değiştirilemez ve silinemez.", soluk: true, boyut: 13));
        Kok.Add(Arayuz.Alan("Tür", _tur));
        Kok.Add(Arayuz.Alan("Açıklama", _aciklama));
        Kok.Add(kaydet);
        Kok.Add(_liste);
    }

    protected override async Task Yukle()
    {
        List<Satir> kayitlar = await Task.Run(GuvenlikVeri.Olaylar);
        Arayuz.Listele(_liste, kayitlar.Select(s => (View)Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Arayuz.Yazi(s.Metin("Tur") + " · " + s.Metin("Zaman"), kalin: true),
                Arayuz.Yazi(s.Metin("Aciklama"), soluk: true, boyut: 13),
                Arayuz.Yazi(s.Metin("Personel"), soluk: true, boyut: 12)
            }
        })).ToList(), "Henüz olay kaydı yok.");
    }

    private async Task Kaydet()
    {
        Sonuc sonuc = await Task.Run(() => GuvenlikVeri.OlayEkle(_oturum.Id, _tur.SelectedItem?.ToString() ?? "Vardiya Teslimi", _aciklama.Text ?? ""));
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

public sealed class AcilSayfasi : TemelSayfa
{
    private readonly Oturum _oturum;
    private readonly Entry _not = Arayuz.Kutu("Kısa not");
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };

    public AcilSayfasi(Oturum oturum) : base("Acil durum")
    {
        _oturum = oturum;
        Button polis = Arayuz.Birincil("Polis");
        polis.Clicked += async (_, _) => await Guvenli(() => Kaydet("Polis"), "Acil durum kaydı oluşturulamadı");
        Button itfaiye = Arayuz.Birincil("İtfaiye");
        itfaiye.Clicked += async (_, _) => await Guvenli(() => Kaydet("İtfaiye"), "Acil durum kaydı oluşturulamadı");
        Kok.Add(Arayuz.Yazi("Bu düğmeler arama yapmaz. Yalnızca kağıt kayıt açar.", soluk: true, boyut: 13));
        Kok.Add(Arayuz.Alan("Not", _not));
        Kok.Add(polis);
        Kok.Add(itfaiye);
        Kok.Add(_liste);
    }

    protected override async Task Yukle()
    {
        List<Satir> kayitlar = await Task.Run(GuvenlikVeri.Aciller);
        Arayuz.Listele(_liste, kayitlar.Select(s => (View)Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Arayuz.Yazi(s.Metin("Tur") + " · " + s.Metin("Zaman"), kalin: true),
                Arayuz.Yazi(string.IsNullOrWhiteSpace(s.Metin("NotMetni")) ? "Not yok" : s.Metin("NotMetni"), soluk: true, boyut: 13)
            }
        })).ToList(), "Henüz acil durum kaydı yok.");
    }

    private async Task Kaydet(string tur)
    {
        Sonuc sonuc = await Task.Run(() => GuvenlikVeri.AcilEkle(_oturum.Id, tur, _not.Text));
        _not.Text = "";
        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }
}

public sealed class BiletSayfasi : TemelSayfa
{
    private readonly Oturum _oturum;
    private readonly Entry _konu = Arayuz.Kutu("Konu");
    private readonly Editor _aciklama = Arayuz.MetinAlani("Açıklama");
    private readonly Entry _sakin = Arayuz.Kutu("Sakin ad soyad");
    private readonly Picker _daire = new Picker { Title = "Daire", TextColor = Tema.Yazi };
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };

    public BiletSayfasi(Oturum oturum) : base("Yönetim bileti")
    {
        _oturum = oturum;
        Button kaydet = Arayuz.Birincil("Bileti ilet");
        kaydet.Clicked += async (_, _) => await Guvenli(Kaydet, "Bilet iletilemedi");
        Kok.Add(Arayuz.Alan("Konu", _konu));
        Kok.Add(Arayuz.Alan("Açıklama", _aciklama));
        Kok.Add(Arayuz.Alan("Sakin", _sakin));
        Kok.Add(Arayuz.Alan("Daire", _daire));
        Kok.Add(kaydet);
        Kok.Add(_liste);
    }

    protected override async Task Yukle()
    {
        List<ListeKalemi> daireler = await Task.Run(() =>
        {
            List<ListeKalemi> liste = new List<ListeKalemi> { new ListeKalemi(0, "(Daire seçilmedi)") };
            liste.AddRange(Veritabani.DaireListesi());
            return liste;
        });
        List<Satir> kayitlar = await Task.Run(() => GuvenlikVeri.Biletlerim(_oturum.Id));
        PickerDoldur(_daire, daireler);
        Arayuz.Listele(_liste, kayitlar.Select(s => (View)Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Arayuz.Yazi(s.Metin("Konu"), kalin: true),
                Arayuz.Yazi(s.Metin("Sakin") + "  " + s.Metin("Daire"), soluk: true, boyut: 13),
                Arayuz.Yazi(s.Metin("Zaman"), soluk: true, boyut: 12)
            }
        })).ToList(), "Henüz bilet yok.");
    }

    private async Task Kaydet()
    {
        int? daireId = _daire.SelectedItem is ListeKalemi daire && daire.Id > 0 ? daire.Id : null;
        Sonuc sonuc = await Task.Run(() => GuvenlikVeri.BiletEkle(_oturum.Id, _konu.Text ?? "", _aciklama.Text ?? "", _sakin.Text, daireId));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        _konu.Text = "";
        _aciklama.Text = "";
        _sakin.Text = "";
        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }
}

public sealed class AracSayfasi : TemelSayfa
{
    private readonly Entry _plaka = Arayuz.Kutu("Plaka");
    private readonly Picker _daire = new Picker { Title = "Daire", TextColor = Tema.Yazi };
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };
    private readonly int _kullaniciId;

    public AracSayfasi(Oturum oturum) : base("Kayıtlı araçlar")
    {
        _kullaniciId = oturum.Id;
        Button kaydet = Arayuz.Birincil("Aracı kaydet");
        kaydet.Clicked += async (_, _) => await Guvenli(Kaydet, "Araç kaydı oluşturulamadı");
        Kok.Add(Arayuz.Yazi("Güvenlik bu listeden yalnızca daire ve sakin adını görür.", soluk: true, boyut: 13));
        Kok.Add(Arayuz.Alan("Plaka", _plaka));
        Kok.Add(Arayuz.Alan("Daire", _daire));
        Kok.Add(kaydet);
        Kok.Add(_liste);
    }

    protected override async Task Yukle()
    {
        (List<ListeKalemi> daireler, List<Satir> araclar) = await Task.Run(() => (Veritabani.DaireListesi(), GuvenlikVeri.Araclar()));
        PickerDoldur(_daire, daireler);
        List<View> kartlar = new List<View>();
        foreach (Satir satir in araclar)
        {
            int id = satir.Tam("AracID");
            Button sil = Arayuz.Ikincil("Sil");
            sil.Clicked += async (_, _) => await Guvenli(() => Sil(id), "Araç kaydı silinemedi");
            kartlar.Add(Arayuz.Kart(new VerticalStackLayout
            {
                Spacing = 4,
                Children =
                {
                    Arayuz.Yazi(satir.Metin("Plaka") + " · " + satir.Metin("Daire"), kalin: true),
                    sil
                }
            }));
        }

        Arayuz.Listele(_liste, kartlar, "Henüz kayıtlı araç yok.");
    }

    private async Task Kaydet()
    {
        int daireId = _daire.SelectedItem is ListeKalemi daire ? daire.Id : 0;
        Sonuc sonuc = await Task.Run(() => GuvenlikVeri.AracEkle(_kullaniciId, GuvenlikVeri.Plaka(_plaka.Text ?? ""), daireId));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        _plaka.Text = "";
        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }

    private async Task Sil(int id)
    {
        bool onay = await DisplayAlert("Araç", "Seçili araç kaydı silinsin mi?", "Sil", "Vazgeç");
        if (!onay)
        {
            return;
        }

        await Task.Run(() => GuvenlikVeri.AracSil(id));
        await Yukle();
    }
}

public sealed class BiletListesiSayfasi : TemelSayfa
{
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };

    public BiletListesiSayfasi() : base("Yönetim biletleri")
    {
        Kok.Add(Arayuz.Yazi("Güvenliğin ilettiği talepler. Kayıtlar salt okunur.", soluk: true, boyut: 13));
        Kok.Add(_liste);
    }

    protected override async Task Yukle()
    {
        List<Satir> kayitlar = await Task.Run(GuvenlikVeri.TumBiletler);
        Arayuz.Listele(_liste, kayitlar.Select(s => (View)Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Arayuz.Yazi(s.Metin("Konu"), kalin: true),
                Arayuz.Yazi(s.Metin("Aciklama"), soluk: true, boyut: 13),
                Arayuz.Yazi(s.Metin("Olusturan") + " · " + s.Metin("Zaman"), soluk: true, boyut: 12)
            }
        })).ToList(), "Henüz bilet yok.");
    }
}

public sealed class GunlukSayfasi : TemelSayfa
{
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };

    public GunlukSayfasi() : base("Güvenlik kayıtları")
    {
        Kok.Add(Arayuz.Yazi("İşlemler tarih, saat ve kullanıcı ile durur. Değiştirilemez.", soluk: true, boyut: 13));
        Kok.Add(_liste);
    }

    protected override async Task Yukle()
    {
        List<Satir> kayitlar = await Task.Run(GuvenlikVeri.Gunluk);
        Arayuz.Listele(_liste, kayitlar.Select(s => (View)Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Arayuz.Yazi(s.Metin("IslemTuru") + " · " + s.Metin("Zaman"), kalin: true),
                Arayuz.Yazi(s.Metin("Personel"), soluk: true, boyut: 13),
                Arayuz.Yazi(s.Metin("Aciklama"), soluk: true, boyut: 12)
            }
        })).ToList(), "Henüz işlem kaydı yok.");
    }
}
