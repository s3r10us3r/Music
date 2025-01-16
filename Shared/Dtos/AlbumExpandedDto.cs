namespace Shared.Dtos;

public class AlbumExpandedDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Artist { get; set; } = "";
    public string Image { get; set; } = "";
    public int Year { get; set; }
    public int ReviewCount { get; set; }
    public double AverageScore { get; set; }
    public List<ReviewDto> Reviews { get; set; } = [];
}