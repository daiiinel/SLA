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
            builder.Services.AddTransient<NuevoRegistroPaso1ViewModel>();
            builder.Services.AddTransient<NuevoRegistroPaso2ViewModel>();
            builder.Services.AddTransient<NuevoRegistroPaso3ViewModel>();
            builder.Services.AddTransient<RevisionRegistroViewModel>();

            builder.Services.AddTransient<NuevoRegistroPaso1Page>();
            builder.Services.AddTransient<NuevoRegistroPaso2Page>();
            builder.Services.AddTransient<NuevoRegistroPaso3Page>();
            builder.Services.AddTransient<RevisionRegistrosPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
