namespace MusicMaui.Services.Extensions
{
    public static class DependencyInjection
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<DataHoldingService>();
            services.AddSingleton<ImageConversionService>();
        }
    }
}
