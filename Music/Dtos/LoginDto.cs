using System.ComponentModel.DataAnnotations;

namespace Music.Dtos;

public class LoginDto
{
    [Required] [Length(6, 128)] public string Username { get; set; } = "";
    [Required] [Length(8, 128)] public string Password { get; set; } = "";
}