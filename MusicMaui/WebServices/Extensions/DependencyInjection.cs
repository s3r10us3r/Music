using MusicMaui.WebServices.Interfaces;

namespace MusicMaui.WebServices.Extensions
{
    public static class DependencyInjection
    {
        public static void AddWebServices(this IServiceCollection services)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:7112/");
            services.AddSingleton(httpClient);
            services.AddTransient<IUserWebService, UserWebService>();
            services.AddTransient<IAlbumWebService, AlbumWebService>();
            services.AddTransient<IReviewWebService, ReviewWebService>();
        }
    }
}
