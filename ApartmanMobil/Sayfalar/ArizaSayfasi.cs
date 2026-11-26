namespace ApartmanMobil;

public sealed class ArizaSayfasi : TemelSayfa
{
    private readonly Oturum _oturum;
    private readonly bool _sakin;
    private readonly Picker _daire = new Picker { Title = "Daire", TextColor = Tema.Yazi, TitleColor = Tema.YaziSoluk, BackgroundColor = Colors.Transparent, HeightRequest = 44 };
    private readonly Picker _alan = Arayuz.Secim("Alan", new List<string> { "Daire içi", "Merdiven", "Asansör", "Otopark", "Çatı", "Bahçe", "Diğer" });
    private readonly Picker _kategori = Arayuz.Secim("Kategori", new List<string> { "Tesisat", "Elektrik", "Asansör", "Doğalgaz", "Diğer" });
    private readonly Picker _oncelik = Arayuz.Secim("Öncelik", new List<string> { "Düşük", "Normal", "Yüksek" }, 1);
    private readonly Editor _aciklama = Arayuz.MetinAlani("Arızayı anlatın");
    private readonly Picker _durum = Arayuz.Secim("Durum", new List<string> { Veritabani.ArizaBeklemede, Veritabani.ArizaIslemde, Veritabani.ArizaTamamlandi }, 1);
    private readonly Entry _cozum = Arayuz.Kutu("Çözüm notu");
    private readonly VerticalStackLayout _liste = new VerticalStackLayout { Spacing = 8 };
    private List<Satir> _kayitlar = new List<Satir>();
    private int _seciliId;

    public ArizaSayfasi(Oturum oturum) : base(oturum.SakinMi ? "Arıza Bildir" : "Arıza")
    {
        _oturum = oturum;
        _sakin = oturum.SakinMi || (!oturum.YoneticiMi && !oturum.TeknisyenMi);
        Kok.Add(Arayuz.Yazi(_sakin
            ? "Tüm arızaları görüntüleyin; kendi daireniz için yeni bildirim oluşturun."
            : "Bildirimleri görüntüleyin, durumunu ve çözüm notunu güncelleyin.", soluk: true, boyut: 13));

        if (_sakin)
        {
            Button kaydet = Arayuz.Birincil("Bildirimi kaydet");
            kaydet.Clicked += async (_, _) => await Guvenli(Kaydet, "Bildirim kaydedilemedi");
            Kok.Add(Arayuz.Alan("Daire", _daire));
            Kok.Add(Arayuz.Alan("Alan", _alan));
            Kok.Add(Arayuz.Alan("Kategori", _kategori));
            Kok.Add(Arayuz.Alan("Öncelik", _oncelik));
            Kok.Add(Arayuz.Alan("Açıklama", _aciklama));
            Kok.Add(kaydet);
        }
        else
        {
            Button guncelle = Arayuz.Birincil("Durumu güncelle");
            guncelle.Clicked += async (_, _) => await Guvenli(Guncelle, "Güncelleme kaydedilemedi");
            Kok.Add(Arayuz.Yazi("Listeden bir arıza seçin.", soluk: true, boyut: 13));
            Kok.Add(Arayuz.Alan("Durum", _durum));
            Kok.Add(Arayuz.Alan("Çözüm notu", _cozum));
            Kok.Add(guncelle);
        }

        Kok.Add(Arayuz.Baslik("BİLDİRİMLER"));
        Kok.Add(_liste);
    }

    protected override async Task Yukle()
    {
        int kullaniciId = _oturum.Id;
        (List<Satir> arizalar, List<ListeKalemi> daireler) = await Task.Run(() => (Listeler.Arizalar(), Veritabani.KullaniciDaireleri(kullaniciId)));
        _kayitlar = arizalar;
        if (_sakin)
        {
            PickerDoldur(_daire, daireler);
        }
        else if (_kayitlar.All(s => s.Tam("ArizaID") != _seciliId))
        {
            _seciliId = 0;
        }

        Ciz();
    }

    private void Ciz()
    {
        List<View> kartlar = new List<View>();
        foreach (Satir satir in _kayitlar)
        {
            Satir secim = satir;
            bool secili = !_sakin && secim.Tam("ArizaID") == _seciliId;
            kartlar.Add(Arayuz.Kart(new VerticalStackLayout
            {
                Spacing = 2,
                Children =
                {
                    Arayuz.Yazi(secim.Metin("Daire") + " · " + secim.Metin("Durum"), kalin: true),
                    Arayuz.Yazi(secim.Metin("Alan") + " · " + secim.Metin("Kategori") + " · " + secim.Metin("Oncelik"), soluk: true, boyut: 13),
                    Arayuz.Yazi(secim.Metin("Aciklama")),
                    Arayuz.Yazi(secim.Metin("Bildiren") + " · " + secim.Metin("OlusturmaTarihi"), soluk: true, boyut: 12),
                    Arayuz.Yazi(string.IsNullOrWhiteSpace(secim.Metin("CozumNotu")) ? "" : "Çözüm: " + secim.Metin("CozumNotu"), soluk: true, boyut: 12)
                }
            }, _sakin ? null : () =>
            {
                _seciliId = secim.Tam("ArizaID");
                _cozum.Text = secim.Metin("CozumNotu");
                string durum = secim.Metin("Durum");
                int indeks = _durum.Items.IndexOf(durum);
                if (indeks >= 0)
                {
                    _durum.SelectedIndex = indeks;
                }

                Ciz();
            }, secili));
        }

        Arayuz.Listele(_liste, kartlar, "Henüz arıza bildirimi yok.");
    }

    private async Task Kaydet()
    {
        int daireId = _daire.SelectedItem is ListeKalemi daire ? daire.Id : 0;
        string? alan = _alan.SelectedItem?.ToString();
        string? kategori = _kategori.SelectedItem?.ToString();
        string? oncelik = _oncelik.SelectedItem?.ToString();
        Sonuc sonuc = await Task.Run(() => Islemler.ArizaEkle(_oturum.Id, daireId, alan, kategori, _aciklama.Text, oncelik));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        _aciklama.Text = "";
        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }

    private async Task Guncelle()
    {
        if (_seciliId == 0)
        {
            await Uyari("Kayıt bulunamadı. Listeden bir arıza seçiniz.");
            return;
        }

        string durum = _durum.SelectedItem?.ToString() ?? Veritabani.ArizaIslemde;
        string cozum = _cozum.Text ?? "";
        Sonuc sonuc = await Task.Run(() => Islemler.ArizaGuncelle(_oturum.Id, _oturum.Rol, _seciliId, durum, cozum));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }
}
