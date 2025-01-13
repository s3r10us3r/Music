using Music.Services.Interfaces;

namespace Music.Services;

public class PasswordCheckingService : IPasswordCheckingService
{
    public bool CheckPassword(string password, out string message)
    {
        if (password.Length < 8)
        {
            message = "Password is too short";
            return false;
        }

        if (password.Length > 128)
        {
            message = "Password is too long";
            return false;
        }

        message = "";
        return true;
    }
}