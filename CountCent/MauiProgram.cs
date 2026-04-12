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

            // transient dependency service
            // with this DIed, we can now assign the main page prop
            builder.Services.AddTransient<MainPage>();

            // inject LocalDbService as a singleton depedency service
            builder.Services.AddSingleton<LocalDbService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
