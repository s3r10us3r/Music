namespace Music.Services.Interfaces;

public interface IPasswordCheckingService
{
    public bool CheckPassword(string password, out string message);
}