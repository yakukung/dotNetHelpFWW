using Microsoft.Extensions.Logging;
using MauiAppPro.ViewsModel; // เพิ่ม namespace

namespace MauiAppPro;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        // ลงทะเบียน HomeViewModel ใน DI Container
        builder.Services.AddTransient<HomeViewModel>();

        return builder.Build();
    }
}