using Microsoft.AspNetCore.Identity;
using Music.Services.Interfaces;

namespace Music.Services.Extensions;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordCheckingService, PasswordCheckingService>();
        services.AddScoped<AlbumService>();
        services.AddScoped<LoginService>();
        services.AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();
    }
}