using Shared.Dtos;

namespace Music.Models;

public class IndexModel
{
    public int PageNumber { get; set; }
    public int PageCount { get; set; }
    public List<AlbumDto> Albums { get; set; } = [];
}