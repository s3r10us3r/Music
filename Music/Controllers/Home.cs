using Microsoft.AspNetCore.Mvc;
using Music.Dtos;
using Music.Models;
using Music.Services;

namespace Music.Controllers;

public class Home : Controller
{
    private readonly AlbumService _albumService;

    public Home(AlbumService albumService)
    {
        _albumService = albumService;
    }
    
    public async Task<IActionResult> Index(int page = 1, string search = "")
    {
        /*
        var request = new AlbumPagedRequest()
        {
            PageNumber = page,
            PageSize = 10,
            SearchQuery = "",
            SortOrder = "ascending",
            SortProperty = ""
        }
        var albums = _albumService.GetPage()
        */
        var model = new IndexModel();
        return View(model);
    }
}