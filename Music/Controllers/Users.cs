using Dal.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Music.Controllers.ApiControllers;
using Music.Services;
using Shared.Dtos;

namespace Music.Controllers;

public class Users : Controller
{
    private readonly IUserRepo _userRepo;
    private readonly AlbumService _albumService;

    public Users(IUserRepo userRepo, AlbumService albumService)
    {
        _userRepo = userRepo;
        _albumService = albumService;
    }
    
    public async Task<IActionResult> Profile(int id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null)
            return NotFound();
        var reviews = user.Reviews.OrderByDescending(r => r.DateTime);
        var reviewAlbumPairs = new List<ReviewAlbumPair>();
        foreach (var review in reviews)
        {
            var dto = await _albumService.GetAlbumDto(review.Album);
            reviewAlbumPairs.Add(new()
            {
                Album = dto,
                Review = new ReviewDto(review)
            });
        }
        var model = new UserExpandedDto() { Username = user.Username, ReviewAlbumPairs = reviewAlbumPairs };
        return View(model);
    }

    public async Task<IActionResult> Search(string query = "")
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return View(new List<UserDto>());
        }
            
        var users = await _userRepo.FilterAsync(u => u.Username.ToLower().Contains(query));
        var first100 = users.Take(100).Select(u => new UserDto(u)).ToList();
        return View(first100);
    }
}