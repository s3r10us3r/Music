using Microsoft.Extensions.Logging;
using MusicMaui.Services.Extensions;
using MusicMaui.ViewModels.Extensions;
using MusicMaui.Views.Extensions;
using MusicMaui.WebServices.Extensions;

namespace MusicMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            try
            {
                var builder = MauiApp.CreateBuilder();
                builder
                    .UseMauiApp<App>()
                    .ConfigureFonts(fonts =>
                    {
                        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    });
                builder.Services.AddServices();
                builder.Services.AddViews();
                builder.Services.AddViewModels();
                builder.Services.AddWebServices();

#if DEBUG
                builder.Logging.AddDebug();
#endif

                return builder.Build();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR ON APPLICATION STARTU P!!!!!!!!!!!!!!!!!!!", ex);
                throw;
            }
        }
    }
}
