using System.ComponentModel.DataAnnotations;

namespace Music.Dtos;

public class NewAlbumDto
{
    [Required] [Length(1, 256)] public string Name { get; set; } = "";
    [Required] [Length(1, 256)] public string Artist { get; set; } = "";
    [Required] public string Image { get; set; } = ""; //b64
    [Required] public int Year { get; set; }
}