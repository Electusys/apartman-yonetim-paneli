namespace ApartmanMobil;

public sealed class GirisSayfasi : ContentPage
{
    private readonly Entry _kullanici = Arayuz.Kutu("Kullanıcı adı");
    private readonly Entry _sifre = Arayuz.Kutu("Şifre", sifre: true);
    private readonly Button _giris = Arayuz.Birincil("Giriş Yap");

    public GirisSayfasi()
    {
        Title = "Giriş";
        Opacity = 0;
        BackgroundColor = Tema.ArkaPlan;
        NavigationPage.SetHasNavigationBar(this, false);

        _giris.Clicked += async (_, _) => await GirisYap();
        Button kayit = Arayuz.Ikincil("Kayıt Ol");
        kayit.Clicked += async (_, _) => await Navigation.PushAsync(new KayitSayfasi());
        Button baglanti = Arayuz.Ikincil("Bağlantı ayarı");
        baglanti.Clicked += async (_, _) => await Navigation.PushAsync(new BaglantiSayfasi());

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Padding = new Thickness(24, 56, 24, 24),
                Spacing = 14,
                Children =
                {
                    new Image
                    {
                        Source = "logo.png",
                        HeightRequest = 72,
                        WidthRequest = 72,
                        HorizontalOptions = LayoutOptions.Start,
                        Aspect = Aspect.AspectFit
                    },
                    Arayuz.Yazi("Apartman Yönetim", boyut: 28, kalin: true),
                    Arayuz.Yazi("Sakin kayıtları açık. Yönetici hesapları yalnızca sistem yöneticisi tarafından oluşturulur.", soluk: true, boyut: 14),
                    Arayuz.Alan("Kullanıcı adı", _kullanici),
                    Arayuz.Alan("Şifre", _sifre),
                    _giris,
                    kayit,
                    baglanti,
                    Arayuz.Yazi("Veritabanı bilgisayarda durur. Telefon aynı Wi-Fi ağında olmalı.", soluk: true, boyut: 12)
                }
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (Opacity < 1)
        {
            await this.FadeTo(1, 180, Easing.CubicOut);
        }

        _ = AdminHazirla();
    }

    private async Task AdminHazirla()
    {
        try
        {
            await Task.Run(BaglantiAyar.UlasilabilirAdresiSec);
            await Task.Run(Veritabani.AdminHesabiniHazirla);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", "Admin hesabı hazırlanırken bir hata oluştu:\n" + ex.Message + Veritabani.BaglantiIpucu(ex), "Tamam");
        }
    }

    private async Task GirisYap()
    {
        _giris.IsEnabled = false;
        try
        {
            GirisSonucu sonuc = await Task.Run(() => Islemler.Giris(_kullanici.Text, _sifre.Text));
            if (!sonuc.Basarili || sonuc.Oturum is null)
            {
                await DisplayAlert("Uyarı", sonuc.Mesaj, "Tamam");
                return;
            }

            _sifre.Text = "";
            Arayuz.KokeGit(new AnaKabuk(sonuc.Oturum));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", "Giriş sırasında bir hata oluştu:\n" + ex.Message + Veritabani.BaglantiIpucu(ex), "Tamam");
        }
        finally
        {
            _giris.IsEnabled = true;
        }
    }
}
