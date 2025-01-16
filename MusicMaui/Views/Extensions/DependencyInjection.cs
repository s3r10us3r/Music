namespace MusicMaui.Views.Extensions
{
    public static class DependencyInjection
    {
        public static void AddViews(this IServiceCollection services)
        {
            services.AddTransient<LoginPage>();
            services.AddTransient<RegisterPage>();
            services.AddTransient<MainPage>();
            services.AddTransient<LogoutPage>();
            services.AddTransient<AddAlbumPage>();
            services.AddTransient<AlbumDetailsPage>();
            services.AddTransient<UserProfilePage>();
        }
    }
}
