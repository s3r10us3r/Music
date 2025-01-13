using System.ComponentModel.DataAnnotations;

namespace Music.Dtos;

public class NewReviewDto
{
    [Required] [Range(1,5)] public int Value { get; set; }
    [Required] public string Message { get; set; } = "";
    public int AlbumId { get; set; }
}