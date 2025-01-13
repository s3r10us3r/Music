using Models;

namespace Music.Dtos;

public class ReviewDto
{
    public string PosterName { get; set; } = "";
    public int Value { get; set; }
    public string Message { get; set; } = "";

    public ReviewDto()
    {
    }

    public ReviewDto(Review review)
    {
        PosterName = review.User.Username;
        Value = review.Value;
        Message = review.Message;
    }
}