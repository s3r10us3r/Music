namespace Shared.Dtos;

public class LoginResponseDto
{
    public string Jwt { get; set; } = "";
    public DateTime ExpiryDate { get; set; }

    public LoginResponseDto(string jwt, DateTime expiryDate)
    {
        Jwt = jwt;
        ExpiryDate = expiryDate;
    }

    public LoginResponseDto()
    {
    }
}