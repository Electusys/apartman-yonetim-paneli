namespace ApartmanMobil;

public sealed class AnaKabuk : Shell
{
    public AnaKabuk(Oturum oturum)
    {
        FlyoutBackgroundColor = Tema.Yuzey;
        BackgroundColor = Tema.ArkaPlan;
        FlyoutBehavior = FlyoutBehavior.Flyout;
        Shell.SetForegroundColor(this, Tema.Yazi);
        Shell.SetBackgroundColor(this, Tema.Yuzey);
        Shell.SetTitleColor(this, Tema.Yazi);

        FlyoutHeader = new VerticalStackLayout
        {
            Padding = new Thickness(20, 52, 20, 12),
            Spacing = 4,
            BackgroundColor = Tema.Yuzey,
            Children =
            {
                new Image
                {
                    Source = "logo.png",
                    HeightRequest = 52,
                    WidthRequest = 52,
                    HorizontalOptions = LayoutOptions.Start,
                    Aspect = Aspect.AspectFit
                },
                Arayuz.Yazi(oturum.AdSoyad, boyut: 18, kalin: true),
                new Label
                {
                    Text = oturum.Rol,
                    TextColor = Tema.Vurgu,
                    FontSize = 13
                }
            }
        };

        Button cikis = Arayuz.Ikincil("Çıkış Yap");
        cikis.Clicked += async (_, _) =>
        {
            if (oturum.GuvenlikMi)
            {
                try
                {
                    await Task.Run(() => GuvenlikVeri.Yaz(oturum.Id, "Oturum kapatıldı", "Güvenlik oturumu kapatıldı."));
                }
                catch
                {
                    // Çıkış, günlük yazılamasa da tamamlanır.
                }
            }

            Arayuz.KokeGit(Arayuz.GirisKoku());
        };
        FlyoutFooter = new VerticalStackLayout
        {
            Padding = new Thickness(16, 8, 16, 28),
            BackgroundColor = Tema.Yuzey,
            Children = { cikis }
        };

        ItemTemplate = new DataTemplate(() =>
        {
            Label etiket = new Label
            {
                FontSize = 16,
                TextColor = Tema.Yazi,
                VerticalOptions = LayoutOptions.Center
            };
            etiket.SetBinding(Label.TextProperty, "Title");
            return new Border
            {
                BackgroundColor = Colors.Transparent,
                StrokeThickness = 0,
                Padding = new Thickness(8, 12),
                Content = etiket
            };
        });

        if (oturum.AdminMi)
        {
            Menu("Yönetici Oluştur", () => new AdminSayfasi(oturum));
        }
        else if (oturum.YoneticiMi)
        {
            Menu("Panel", () => new PanelSayfasi(oturum));
            Menu("Blok ve Daire", () => new BlokDaireSayfasi());
            Menu("Sakinler", () => new SakinSayfasi());
            Menu("Aidat", () => new AidatSayfasi(oturum));
            Menu("Ödemeler", () => new OdemeSayfasi(oturum));
            Menu("Giderler", () => new GiderSayfasi(oturum));
            Menu("Duyurular", () => new DuyuruSayfasi(oturum));
            Menu("Arıza", () => new ArizaSayfasi(oturum));
            Menu("Personel", () => new PersonelSayfasi());
            Menu("Kayıtlı Araçlar", () => new AracSayfasi(oturum));
            Menu("Yönetim Biletleri", () => new BiletListesiSayfasi());
            Menu("Güvenlik Kayıtları", () => new GunlukSayfasi());
        }
        else if (oturum.MuhasebeciMi)
        {
            Menu("Panel", () => new PanelSayfasi(oturum));
            Menu("Ödemeler", () => new OdemeSayfasi(oturum));
            Menu("Giderler", () => new GiderSayfasi(oturum));
            Menu("Duyurular", () => new DuyuruSayfasi(oturum));
        }
        else if (oturum.TeknisyenMi)
        {
            Menu("Panel", () => new PanelSayfasi(oturum));
            Menu("Arıza", () => new ArizaSayfasi(oturum));
            Menu("Duyurular", () => new DuyuruSayfasi(oturum));
        }
        else if (oturum.GuvenlikMi)
        {
            Menu("Panel", () => new PanelSayfasi(oturum));
            Menu("Ziyaret", () => new ZiyaretSayfasi(oturum));
            Menu("Kargo", () => new KargoSayfasi(oturum));
            Menu("Sakin Rehberi", () => new RehberSayfasi(oturum));
            Menu("Plaka ve Park", () => new PlakaSayfasi(oturum));
            Menu("Olay Raporu", () => new OlaySayfasi(oturum));
            Menu("Acil Durum", () => new AcilSayfasi(oturum));
            Menu("Yönetim Bileti", () => new BiletSayfasi(oturum));
        }
        else
        {
            Menu("Panel", () => new PanelSayfasi(oturum));
            Menu("Duyurular", () => new DuyuruSayfasi(oturum));
            Menu("Aidatlarım", () => new AidatlarimSayfasi(oturum));
            Menu("Arıza Bildir", () => new ArizaSayfasi(oturum));
        }

        Menu("Bağlantı", () => new BaglantiSayfasi());
    }

    private void Menu(string baslik, Func<Page> sayfa)
    {
        Items.Add(new FlyoutItem
        {
            Title = baslik,
            Items =
            {
                new ShellContent
                {
                    Title = baslik,
                    ContentTemplate = new DataTemplate(() => sayfa())
                }
            }
        });
    }
}
