namespace Shared.Dtos;

public class MessageDto
{
    public string Message { get; set; } = "";

    public MessageDto()
    {
    }

    public MessageDto(string message)
    {
        Message = message;
    }
}