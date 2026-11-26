using System.Globalization;

namespace ApartmanMobil;

public sealed class OdemeSayfasi : TemelSayfa
{
    private readonly Oturum _oturum;
    private readonly Entry _tutar = Arayuz.Kutu("0", klavye: Keyboard.Numeric);
    private readonly DatePicker _tarih = Arayuz.Tarih();
    private readonly Picker _yontem = Arayuz.Secim("Yöntem", new List<string> { "Nakit", "Havale", "EFT", "Kredi Kartı" });
    private readonly Label _ozet = new Label { Text = "Bir borç satırı seçiniz.", TextColor = Tema.Vurgu, FontSize = 13 };
    private readonly VerticalStackLayout _borclar = new VerticalStackLayout { Spacing = 8 };
    private readonly VerticalStackLayout _odemeler = new VerticalStackLayout { Spacing = 8 };
    private List<Satir> _borcKayitlari = new List<Satir>();
    private int _seciliId;

    public OdemeSayfasi(Oturum oturum) : base("Ödemeler")
    {
        _oturum = oturum;
        Button kaydet = Arayuz.Birincil("Ödemeyi kaydet");
        kaydet.Clicked += async (_, _) => await Guvenli(Kaydet, "Ödeme kaydedilemedi");

        Kok.Add(Arayuz.Yazi("Daire aidat borcunu seçip ödemeyi kaydedin.", soluk: true, boyut: 13));
        Kok.Add(Arayuz.Baslik("BORÇLAR"));
        Kok.Add(_borclar);
        Kok.Add(_ozet);
        Kok.Add(Arayuz.Alan("Ödeme tutarı", _tutar));
        Kok.Add(Arayuz.Alan("Tarih", _tarih));
        Kok.Add(Arayuz.Alan("Yöntem", _yontem));
        Kok.Add(kaydet);
        Kok.Add(Arayuz.Baslik("ÖDEMELER"));
        Kok.Add(_odemeler);
    }

    protected override async Task Yukle()
    {
        (List<Satir> borclar, List<Satir> odemeler) = await Task.Run(() => (Listeler.AcikBorclar(), Listeler.Odemeler()));
        _borcKayitlari = borclar;
        if (_borcKayitlari.All(s => s.Tam("AidatBorcID") != _seciliId))
        {
            _seciliId = 0;
            _ozet.Text = "Bir borç satırı seçiniz.";
        }

        CizBorclar();
        Arayuz.Listele(_odemeler, odemeler.Select(s => Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Arayuz.Yazi(s.Metin("Daire") + " · " + s.Metin("Donem"), kalin: true),
                Arayuz.Yazi(s.Metin("Tutar") + " ₺ · " + s.Metin("OdemeYontemi") + " · " + s.Metin("OdemeTarihi"), soluk: true, boyut: 13)
            }
        })).ToList(), "Henüz ödeme yok.");
    }

    private void CizBorclar()
    {
        List<View> kartlar = new List<View>();
        foreach (Satir satir in _borcKayitlari)
        {
            Satir secim = satir;
            bool secili = secim.Tam("AidatBorcID") == _seciliId;
            kartlar.Add(Arayuz.Kart(new VerticalStackLayout
            {
                Spacing = 2,
                Children =
                {
                    Arayuz.Yazi(secim.Metin("Daire") + " · " + secim.Metin("Donem"), kalin: true),
                    Arayuz.Yazi("Toplam " + secim.Metin("Tutar") + " ₺  ·  Kalan " + secim.Metin("KalanTutar") + " ₺", soluk: true, boyut: 13),
                    Arayuz.Yazi(secim.Metin("Durum"), soluk: true, boyut: 12)
                }
            }, () => BorcSec(secim), secili));
        }

        Arayuz.Listele(_borclar, kartlar, "Henüz aidat borcu yok.");
    }

    private void BorcSec(Satir satir)
    {
        _seciliId = satir.Tam("AidatBorcID");
        decimal kalan = satir.Sayi("KalanTutar");
        _ozet.Text = satir.Metin("Daire") + " — Toplam borç: " + satir.Metin("Tutar") + " ₺  |  Kalan: " + satir.Metin("KalanTutar") + " ₺";
        if (kalan > 0)
        {
            _tutar.Text = kalan.ToString("0.##", new CultureInfo("tr-TR"));
        }

        CizBorclar();
    }

    private async Task Kaydet()
    {
        Satir? satir = _borcKayitlari.FirstOrDefault(s => s.Tam("AidatBorcID") == _seciliId);
        if (satir is null)
        {
            await Uyari("Önce bir daire / aidat kaydı seçiniz.");
            return;
        }

        if (!Arayuz.Tutar(_tutar.Text, out decimal odeme) || odeme <= 0)
        {
            await Uyari("Ödeme tutarı geçersiz.");
            return;
        }

        decimal kalan = satir.Sayi("KalanTutar");
        if (odeme > kalan)
        {
            bool sinirla = await DisplayAlert(
                "Kontrol",
                "Ödeme tutarı kalan borçtan (" + satir.Metin("KalanTutar") + " ₺) büyük. İşlem kalan tutarla sınırlansın mı?",
                "Evet",
                "Hayır");
            if (!sinirla)
            {
                return;
            }

            odeme = kalan;
        }

        if (odeme <= 0)
        {
            await Uyari("Bu borcun kalan tutarı yok.");
            return;
        }

        string yontem = _yontem.SelectedItem?.ToString() ?? "Nakit";
        DateTime tarih = _tarih.Date ?? DateTime.Today;
        Sonuc sonuc = await Task.Run(() => Islemler.OdemeKaydet(_seciliId, odeme, tarih, yontem, _oturum.Id));
        if (!sonuc.Tamam)
        {
            await Uyari(sonuc.Mesaj);
            return;
        }

        await Bilgi(sonuc.Mesaj);
        await Yukle();
    }
}
