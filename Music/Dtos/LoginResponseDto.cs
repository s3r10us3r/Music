namespace Music.Dtos;

public class LoginResponseDto
{
    public string Jwt { get; set; } = "";

    public LoginResponseDto(string jwt)
    {
        Jwt = jwt;
    }

    public LoginResponseDto()
    {
    }
}