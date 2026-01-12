using Microsoft.Extensions.Logging;
using SLA.Services;
using SLA.ViewModels;
using SLA.Views;

namespace SLA
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if ANDROID
            builder.Services.AddTransient<IPrintService, SLA.Platforms.Android.AndroidPrintService>();
#endif
            builder.Services.AddTransient<ViewModels.NuevoRegistroPaso3ViewModel>();
            builder.Services.AddTransient<Views.NuevoRegistroPaso3Page>();
            builder.Services.AddTransient<RevisionRegistroViewModel>();
            builder.Services.AddTransient<RevisionRegistrosPage>();
            return builder.Build();
        }
    }
}
