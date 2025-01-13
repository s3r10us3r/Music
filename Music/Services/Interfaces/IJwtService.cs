using Models;

namespace Music.Services.Interfaces;

public interface IJwtService
{
    public string CreateJwt(User user);
}