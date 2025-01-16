using Dal.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Music.Services;
using Shared.Dtos;

namespace Music.Controllers.ApiControllers;

[ApiController]
[Route("/api/[controller]")]
public class AlbumController : ControllerBase
{
    private readonly IAlbumRepo _albumRepo;
    private readonly AlbumService _albumService;

    public AlbumController(IAlbumRepo albumRepo, AlbumService albumService)
    {
        _albumRepo = albumRepo;
        _albumService = albumService;
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] NewAlbumDto dto)
    {
        var album = new Album()
        {
            Name = dto.Name,
            Artist = dto.Artist,
            Image = dto.Image,
            Year = dto.Year,
        };
        await _albumRepo.CreateAsync(album);
        return Ok();
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlbumExpandedDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
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
        return Ok(result);
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AlbumDto>))]
    [HttpPost("getPage")]
    public async Task<IActionResult> GetPaged([FromBody] AlbumPagedRequest request)
    {
        var result = await _albumService.GetPage(request);
        Console.WriteLine($"Results loaded, result count: {result.Count}, search query: {request.SearchQuery}");
        return Ok(result);
    }


    [Authorize]
    [HttpGet("authTest")]
    public async Task<IActionResult> AuthTest()
    {
        return Ok();
    }
}
