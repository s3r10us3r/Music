using Dal.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Dal.Extensions;

public static class DependencyInjection
{
    public static void AddDal(this IServiceCollection services)
    {
        services.AddDbContext<MusicDbContext>();
        services.AddScoped<IAlbumRepo, AlbumRepo>();
        services.AddScoped<IReviewRepo, ReviewRepo>();
        services.AddScoped<IUserRepo, UserRepo>();
    }
}