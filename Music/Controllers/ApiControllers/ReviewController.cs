using System.Security.Claims;
using Dal.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Shared.Dtos;

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
            return Unauthorized(new MessageDto("No sub in jwt"));
        }
        var id = int.Parse(idString);
        var existingReview = await _reviewRepo.FindAsync(r => r.AlbumId == dto.AlbumId && r.UserId == id);
        if (existingReview == null)
        {
            var review = new Review()
            {
                Message = dto.Message,
                Value = dto.Value,
                AlbumId = dto.AlbumId,
                UserId = id
            };
            await _reviewRepo.CreateAsync(review);
        }
        else
        {
            existingReview.Message = dto.Message;
            existingReview.Value = dto.Value;
            await _reviewRepo.UpdateAsync(existingReview);
        }
        return Ok();
    }

    private string? GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId;
    }
}