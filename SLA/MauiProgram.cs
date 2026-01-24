using Microsoft.Extensions.Logging;
using SLA.Services;
using SLA.ViewModels;
using SLA.Views;
using CommunityToolkit.Maui;

namespace SLA
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if ANDROID
            builder.Services.AddTransient<IPrintService, SLA.Platforms.Android.AndroidPrintService>();
#endif
            builder.Services.AddSingleton<NuevoRegistroPaso1ViewModel>();
            builder.Services.AddSingleton<NuevoRegistroPaso2ViewModel>();
            builder.Services.AddSingleton<NuevoRegistroPaso3ViewModel>();
            builder.Services.AddTransient<RevisionRegistroViewModel>();

            builder.Services.AddSingleton<NuevoRegistroPaso1Page>();
            builder.Services.AddSingleton<NuevoRegistroPaso2Page>();
            builder.Services.AddSingleton<NuevoRegistroPaso3Page>();
            builder.Services.AddTransient<RevisionRegistrosPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
