namespace MusicMaui.ViewModels.Extensions
{
    internal static class DependencyInjection
    {
        public static void AddViewModels(this IServiceCollection services)
        {
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<LogoutViewModel>();
            services.AddTransient<AddAlbumViewModel>();
            services.AddTransient<AlbumDetailsViewModel>();
            services.AddTransient<UserProfileViewModel>();
        }
    }
}
