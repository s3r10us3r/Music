using Dal;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using Shared.Dtos;

namespace Music.Services;

public class AlbumService
{
    private readonly MusicDbContext _db;
    
    public AlbumService(MusicDbContext db)
    {
        _db = db;
    }
    
    public async Task<double> GetAverageScore(int id)
    {
        var reviewSet = _db.Set<Review>();
        var score = await reviewSet.Where(r => r.AlbumId == id).Select(r => (double?)r.Value).AverageAsync() ?? 0.0;
        return Math.Round(score, 2);
    }

    public async Task<int> GetReviewCount(int id)
    {
        var reviewSet = _db.Set<Review>();
        return await reviewSet.Where(r => r.AlbumId == id).CountAsync();
    }

    private readonly Func<Album, string> _nameSelector = a => a.Name;
    private readonly Func<Album, string> _artistSelector = a => a.Artist;
    private readonly Func<Album, int> _yearSelector = a => a.Year;
    
    public async Task<List<AlbumDto>> GetPage(AlbumPagedRequest request)
    {
        var start = (request.PageNumber - 1) * request.PageSize;
        var end = request.PageNumber * request.PageSize - 1;
        
        var albumSet = _db.Set<Album>();
        var filtered =
            albumSet.Where(a => a.Name.ToLower().Contains(request.SearchQuery.ToLower()) || a.Artist.ToLower().Contains(request.SearchQuery.ToLower()));
        
        if (request.SortProperty == "AverageScore")
        {
            var tasksBlock = await filtered.ToListAsync();
            var dtos = await GetAlbumDtoList(tasksBlock);
            return request.SortOrder.ToLower() switch
            {
                "descending" => dtos.OrderByDescending(d => d.AverageScore).Take(new Range(start, end)).ToList(),
                "ascending" => dtos.OrderByDescending(d => d.AverageScore).Take(new Range(start, end)).ToList(),
            };
        }
        
        var sorted = request.SortProperty.ToLower() switch
        {
            "name" => SortInStringDirection(filtered, _nameSelector, request.SortOrder),
            "artist" => SortInStringDirection(filtered, _artistSelector, request.SortOrder),
            "year" => SortInStringDirection(filtered, _yearSelector, request.SortOrder),
            _ => filtered
        };
        
        var albumList = sorted.Take(new Range(start, end)).ToList();
        var result = await GetAlbumDtoList(albumList);
        return result.ToList();
    }

    public async Task<List<AlbumDto>> GetAlbumDtoList(List<Album> albums)
    {
        List<AlbumDto> dtos = [];
        foreach (var album in albums)
        {
            var dto = await GetAlbumDto(album);
            dtos.Add(dto);
        }

        return dtos;
    }
    
    public async Task<AlbumDto> GetAlbumDto(Album album)
    {
        return new AlbumDto()
        {
            Id = album.Id,
            Artist = album.Artist,
            Image = album.Image,
            Name = album.Name,
            Year = album.Year,
            AverageScore = await GetAverageScore(album.Id),
            ReviewCount = await GetReviewCount(album.Id)
        };
    }
    
    private IEnumerable<Album> SortInStringDirection<TKey>(IQueryable<Album> albums, Func<Album, TKey> selector, string direction)
    {
        if (direction.Equals("descending", StringComparison.CurrentCultureIgnoreCase))
        {
            return albums.OrderByDescending(selector);
        }
        else
        {
            return albums.OrderBy(selector);
        }
    }
}