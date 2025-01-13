using System.Security.Claims;
using Dal.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Music.Dtos;

namespace Music.Controllers.ApiControllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : Controller
{
    private readonly IReviewRepo _reviewRepo;

    public ReviewController(IReviewRepo reviewRepo)
    {
        _reviewRepo = reviewRepo;
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] NewReviewDto dto)
    {
        var idString = GetUserId();
        if (idString == null)
        {
            return Unauthorized();
        }

        var id = int.Parse(idString);
        var review = new Review()
        {
            Message = dto.Message,
            Value = dto.Value,
            AlbumId = dto.AlbumId,
            UserId = id
        };
        await _reviewRepo.CreateAsync(review);
        return Ok();
    }

    private string? GetUserId()
    {
        var userId = User.FindFirstValue("sub");
        return userId;
    }
}