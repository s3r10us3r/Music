using Models;

namespace Music.Dtos;

public class UserExpandedDto
{
    public string Username { get; set; } = "";
    public List<ReviewAlbumPair> ReviewAlbumPairs { get; set; } = [];
}