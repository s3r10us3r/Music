using Models;

namespace Music.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public int ReviewCount;
    
    public UserDto()
    {
    }

    public UserDto(User user)
    {
        Id = user.Id;
        Username = user.Username;
        ReviewCount = user.Reviews.Count;
    }
}