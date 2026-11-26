namespace ApartmanMobil;

public sealed class PanelSayfasi : TemelSayfa
{
    private readonly Grid _olculer = new Grid
    {
        ColumnSpacing = 10,
        RowSpacing = 10,
        ColumnDefinitions =
        {
            new ColumnDefinition(GridLength.Star),
            new ColumnDefinition(GridLength.Star)
        },
        RowDefinitions =
        {
            new RowDefinition(GridLength.Auto),
            new RowDefinition(GridLength.Auto)
        }
    };
    private readonly VerticalStackLayout _duyurular = new VerticalStackLayout { Spacing = 8 };
    private readonly Oturum _oturum;
    private readonly Label _altBaslik;

    public PanelSayfasi(Oturum oturum) : base("Panel")
    {
        _oturum = oturum;
        _altBaslik = Arayuz.Baslik(_oturum.GuvenlikMi ? "SON GÜVENLİK İŞLEMLERİ" : "SON DUYURULAR / GÖREVLER");
        Kok.Add(Arayuz.Yazi("Hoş geldiniz, " + oturum.AdSoyad, boyut: 22, kalin: true));
        Kok.Add(Arayuz.Yazi("Aktif rol: " + oturum.Rol, soluk: true, boyut: 13));
        Kok.Add(_olculer);
        Kok.Add(_altBaslik);
        Kok.Add(_duyurular);
    }

    protected override async Task Yukle()
    {
        bool guvenlik = _oturum.GuvenlikMi;
        PanelVerisi veri = await Task.Run(() => guvenlik ? GuvenlikVeri.Panel(_oturum.Id) : Listeler.Panel());
        _olculer.Children.Clear();
        _olculer.Add(Arayuz.Olcu(guvenlik ? "Ziyaret" : "Blok", veri.Blok, guvenlik ? "Bugünkü giriş" : "Kayıtlı blok"), 0, 0);
        _olculer.Add(Arayuz.Olcu(guvenlik ? "Kargo" : "Daire", veri.Daire, guvenlik ? "Teslim alınan" : "Toplam daire"), 1, 0);
        _olculer.Add(Arayuz.Olcu(guvenlik ? "Park" : "Sakin", veri.Sakin, guvenlik ? "Aktif izin" : "Aktif sakin"), 0, 1);
        _olculer.Add(Arayuz.Olcu(guvenlik ? "Bilet" : "Açık arıza", veri.Ariza, guvenlik ? "İlettiklerim" : "Bekleyen iş"), 1, 1);

        List<View> kartlar = veri.Duyurular.Select(s => (View)Arayuz.Kart(new VerticalStackLayout
        {
            Spacing = 4,
            Children =
            {
                Arayuz.Yazi(s.Metin("Gorev"), kalin: true),
                Arayuz.Yazi(s.Metin("Tarih"), soluk: true, boyut: 12)
            }
        })).ToList();
        Arayuz.Listele(_duyurular, kartlar, "Henüz duyuru yok.");
    }
}
