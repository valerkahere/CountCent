using CountCent.Services;
using Microsoft.Extensions.Logging;

namespace CountCent
{
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

            // UI pages
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AnalysisPage>();

            // DB Service
            builder.Services.AddSingleton<LocalDbService>();

            //  HttpClient setup for Frankfurter API
            builder.Services.AddHttpClient<CurrencyService>(client =>
            {
                client.BaseAddress = new Uri("https://api.frankfurter.dev/v1/");
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
