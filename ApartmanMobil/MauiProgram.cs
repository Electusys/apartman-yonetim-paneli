namespace ApartmanMobil;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        AndroidGirdiRenkleri();

        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();
        return builder.Build();
    }

    private static void AndroidGirdiRenkleri()
    {
#if ANDROID
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("ApartmanGirdi", (handler, _) =>
        {
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            handler.PlatformView.SetTextColor(Android.Graphics.Color.Rgb(236, 237, 232));
            handler.PlatformView.SetHintTextColor(Android.Graphics.Color.Rgb(154, 160, 176));
        });
        Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("ApartmanGirdi", (handler, _) =>
        {
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            handler.PlatformView.SetTextColor(Android.Graphics.Color.Rgb(236, 237, 232));
            handler.PlatformView.SetHintTextColor(Android.Graphics.Color.Rgb(154, 160, 176));
        });
        Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("ApartmanGirdi", (handler, _) =>
        {
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            handler.PlatformView.SetTextColor(Android.Graphics.Color.Rgb(236, 237, 232));
        });
        Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping("ApartmanGirdi", (handler, _) =>
        {
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            handler.PlatformView.SetTextColor(Android.Graphics.Color.Rgb(236, 237, 232));
        });
#endif
    }
}
