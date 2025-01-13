using System.ComponentModel.DataAnnotations;
using Models;

namespace Music.Dtos;

public class AlbumDto
{
    public int Id { get; set; }
    [Required] [Length(1, 256)] public string Name { get; set; } = "";
    [Required] [Length(1, 256)] public string Artist { get; set; } = "";
    [Required] public string Image { get; set; } = ""; //b64
    [Required] public int Year { get; set; }
    [Required] public double AverageScore { get; set; }
    [Required] public int ReviewCount { get; set; }
}