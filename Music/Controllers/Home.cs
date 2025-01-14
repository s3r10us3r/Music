using Dal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Music.Dtos;
using Music.Models;
using Music.Services;

namespace Music.Controllers;

public class Home : Controller
{
    private readonly AlbumService _albumService;
    private readonly MusicDbContext _db;

    public Home(AlbumService albumService, MusicDbContext db)
    {
        _albumService = albumService;
        _db = db;
    }
    
    public async Task<IActionResult> Index(int page = 1, string search = "")
    {
        var albumCount = await _db.Set<Album>().CountAsync();
        var pageCount = 1 + Math.Floor(albumCount / 10.0) + albumCount % 10 == 0 ? 0 : 1;
        var request = new AlbumPagedRequest()
        {
            PageNumber = page,
            PageSize = 10,
            SearchQuery = search,
            SortOrder = "ascending",
            SortProperty = "AverageScore"
        };
        var albums = await _albumService.GetPage(request);
        var model = new IndexModel()
        {
            PageNumber = page,
            Albums = albums,
            PageCount = pageCount
        };
        return View(model);
    }
}