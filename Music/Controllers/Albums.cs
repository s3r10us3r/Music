using Dal.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Music.Dtos;
using Music.Services;

namespace Music.Controllers;

public class Albums : Controller
{
    private readonly IAlbumRepo _albumRepo;
    private readonly AlbumService _albumService;

    public Albums(IAlbumRepo albumRepo, AlbumService albumService)
    {
        _albumRepo = albumRepo;
        _albumService = albumService;
    }
    
    public IActionResult Add()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Show(int id)
    {
        var model = await _albumRepo.GetByIdAsync(id);
        if (model == null)
            return NotFound();
        var result = new AlbumExpandedDto()
        {
            Id = model.Id,
            Name = model.Name,
            Artist = model.Artist,
            Image = model.Image,
            Year = model.Year,
            Reviews = model.Reviews.Select(r => new ReviewDto(r)).ToList(),
            AverageScore = await _albumService.GetAverageScore(id),
            ReviewCount = await _albumService.GetReviewCount(id)
        };
        return View(result);
    }
}