namespace Shared.Dtos;

public class AlbumPagedRequest
{
    //case insensitive name of album to be sorted by
    //accepted values are: Name, Artist, Year (no avg score sorting)
    public string SortProperty { get; set; } = "";
    //case insensitive sort order
    //accepted values are descending, ascending
    public string SortOrder { get; set; } = "";
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    //searches after name and artist properties, empty query means that no search happens
    public string SearchQuery { get; set; } = "";
}