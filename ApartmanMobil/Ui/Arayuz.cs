using System.Globalization;
using Microsoft.Maui.Controls.Shapes;

namespace ApartmanMobil;

public static class Arayuz
{
    public static Page GirisKoku()
    {
        return new NavigationPage(new GirisSayfasi())
        {
            BarBackgroundColor = Tema.Yuzey,
            BarTextColor = Tema.Yazi
        };
    }

    public static void KokeGit(Page sayfa)
    {
        Window? pencere = Application.Current?.Windows.FirstOrDefault();
        if (pencere is not null)
        {
            pencere.Page = sayfa;
        }
    }

    public static Label Yazi(string metin, bool soluk = false, double boyut = 15, bool kalin = false)
    {
        return new Label
        {
            Text = metin,
            TextColor = soluk ? Tema.YaziSoluk : Tema.Yazi,
            FontSize = boyut,
            FontAttributes = kalin ? FontAttributes.Bold : FontAttributes.None,
            LineBreakMode = LineBreakMode.WordWrap
        };
    }

    public static Label Baslik(string metin)
    {
        return new Label
        {
            Text = metin,
            TextColor = Tema.Vurgu,
            FontSize = 13,
            FontAttributes = FontAttributes.Bold
        };
    }

    public static Entry Kutu(string yerTutucu, bool sifre = false, Keyboard? klavye = null)
    {
        return new Entry
        {
            Placeholder = yerTutucu,
            IsPassword = sifre,
            Keyboard = klavye ?? Keyboard.Default,
            HorizontalOptions = LayoutOptions.Fill,
            TextColor = Tema.Yazi,
            PlaceholderColor = Tema.YaziSoluk,
            BackgroundColor = Colors.Transparent,
            HeightRequest = 44
        };
    }

    public static Editor MetinAlani(string yerTutucu)
    {
        return new Editor
        {
            Placeholder = yerTutucu,
            HorizontalOptions = LayoutOptions.Fill,
            TextColor = Tema.Yazi,
            PlaceholderColor = Tema.YaziSoluk,
            BackgroundColor = Colors.Transparent,
            HeightRequest = 96,
            AutoSize = EditorAutoSizeOption.TextChanges
        };
    }

    public static Picker Secim(string baslik, IList<string> ogeler, int secili = 0)
    {
        Picker picker = new Picker
        {
            Title = baslik,
            HorizontalOptions = LayoutOptions.Fill,
            TextColor = Tema.Yazi,
            TitleColor = Tema.YaziSoluk,
            BackgroundColor = Colors.Transparent,
            HeightRequest = 44
        };
        foreach (string oge in ogeler)
        {
            picker.Items.Add(oge);
        }

        if (picker.Items.Count > 0)
        {
            picker.SelectedIndex = Math.Clamp(secili, 0, picker.Items.Count - 1);
        }

        return picker;
    }

    public static DatePicker Tarih(DateTime? gun = null)
    {
        return new DatePicker
        {
            Date = gun ?? DateTime.Today,
            Format = "dd.MM.yyyy",
            HorizontalOptions = LayoutOptions.Fill,
            TextColor = Tema.Yazi,
            BackgroundColor = Colors.Transparent,
            HeightRequest = 44
        };
    }

    public static View Alan(string etiket, View girdi)
    {
        return new VerticalStackLayout
        {
            Spacing = 4,
            Children =
            {
                Yazi(etiket, soluk: true, boyut: 13),
                Cerceve(girdi)
            }
        };
    }

    public static Border Cerceve(View icerik)
    {
        return new Border
        {
            BackgroundColor = Tema.Kart,
            Stroke = Tema.Kenar,
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = new Thickness(12, 2),
            Content = icerik
        };
    }

    public static Button Birincil(string metin)
    {
        return Yumusak(new Button
        {
            Text = metin,
            BackgroundColor = Tema.Vurgu,
            TextColor = Tema.YaziKoyu,
            CornerRadius = 14,
            HeightRequest = 48,
            FontAttributes = FontAttributes.Bold
        });
    }

    public static Button Ikincil(string metin)
    {
        return Yumusak(new Button
        {
            Text = metin,
            BackgroundColor = Tema.Yuzey,
            TextColor = Tema.Yazi,
            BorderColor = Tema.Kenar,
            BorderWidth = 1,
            CornerRadius = 14,
            HeightRequest = 48
        });
    }

    private static Button Yumusak(Button button)
    {
        button.Pressed += (_, _) => _ = button.ScaleTo(0.97, 80, Easing.CubicOut);
        button.Released += (_, _) => _ = button.ScaleTo(1, 140, Easing.CubicOut);
        return button;
    }

    public static Border Kart(View icerik, Action? dokun = null, bool secili = false)
    {
        Border cerceve = new Border
        {
            BackgroundColor = Tema.Kart,
            Stroke = secili ? Tema.Vurgu : Tema.Kenar,
            StrokeThickness = secili ? 2 : 1,
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            Padding = 14,
            Content = icerik
        };
        if (dokun is not null)
        {
            TapGestureRecognizer dokunus = new TapGestureRecognizer();
            dokunus.Tapped += (_, _) => dokun();
            cerceve.GestureRecognizers.Add(dokunus);
        }

        return cerceve;
    }

    public static View Olcu(string ad, string deger, string alt)
    {
        return Kart(new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                Baslik(ad),
                Yazi(deger, boyut: 28, kalin: true),
                Yazi(alt, soluk: true, boyut: 12)
            }
        });
    }

    public static bool Tutar(string? yazi, out decimal tutar)
    {
        tutar = 0;
        if (string.IsNullOrWhiteSpace(yazi))
        {
            return false;
        }

        string temiz = yazi.Trim().Replace("₺", "").Replace(" ", "");
        if (decimal.TryParse(temiz, NumberStyles.Number, new CultureInfo("tr-TR"), out tutar))
        {
            return true;
        }

        return decimal.TryParse(temiz.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out tutar);
    }

    public static void Listele(VerticalStackLayout hedef, IReadOnlyList<View> kartlar, string bosMetin)
    {
        hedef.Children.Clear();
        if (kartlar.Count == 0)
        {
            hedef.Add(Yazi(bosMetin, soluk: true, boyut: 13));
            return;
        }

        foreach (View kart in kartlar)
        {
            hedef.Add(kart);
        }
    }
}

public class TemelSayfa : ContentPage
{
    private bool _yukleniyor;
    protected readonly VerticalStackLayout Kok = new VerticalStackLayout
    {
        Padding = new Thickness(16),
        Spacing = 12
    };

    protected TemelSayfa(string baslik)
    {
        Title = baslik;
        BackgroundColor = Tema.ArkaPlan;
        Shell.SetBackgroundColor(this, Tema.Yuzey);
        Shell.SetForegroundColor(this, Tema.Yazi);
        Shell.SetTitleColor(this, Tema.Yazi);
        Shell.SetNavBarHasShadow(this, false);
        Opacity = 0;
        Content = new ScrollView { Content = Kok };
    }

    protected virtual Task Yukle() => Task.CompletedTask;

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (Opacity < 1)
        {
            await this.FadeTo(1, 180, Easing.CubicOut);
        }

        _ = Guvenli(Yukle, "Veriler yüklenemedi");
    }

    protected async Task Guvenli(Func<Task> islem, string hataBaslik)
    {
        if (_yukleniyor)
        {
            return;
        }

        _yukleniyor = true;
        try
        {
            await islem();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", hataBaslik + "\n" + ex.Message + Veritabani.BaglantiIpucu(ex), "Tamam");
        }
        finally
        {
            _yukleniyor = false;
        }
    }

    protected Task Uyari(string mesaj) => DisplayAlert("Uyarı", mesaj, "Tamam");

    protected Task Bilgi(string mesaj) => DisplayAlert("Bilgi", mesaj, "Tamam");

    protected void PickerDoldur(Picker picker, IReadOnlyList<ListeKalemi> liste)
    {
        int secili = picker.SelectedItem is ListeKalemi kalem ? kalem.Id : 0;
        picker.ItemsSource = liste.ToList();
        picker.ItemDisplayBinding = new Binding(nameof(ListeKalemi.Metin));
        ListeKalemi? bulunan = liste.FirstOrDefault(x => x.Id == secili);
        picker.SelectedItem = bulunan ?? (liste.Count > 0 ? liste[0] : null);
    }
}
