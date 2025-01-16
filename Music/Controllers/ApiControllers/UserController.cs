using Dal.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Music.Services;
using Shared.Dtos;

namespace Music.Controllers.ApiControllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly LoginService _loginService;
    private readonly AlbumService _albumService;
    private readonly IUserRepo _userRepo;

    public UserController(LoginService loginService, IUserRepo userRepo, AlbumService albumService)
    {
        _loginService = loginService;
        _albumService = albumService;
        _userRepo = userRepo;
    }
    
    //potem po zalogowaniu token umieszczasz w headerze w tzw. 'bearer scheme' 
    //informacje o użytkowniku będę sam pobierał z tokenu
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(MessageDto))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _loginService.Login(dto);
        if (result.IsSuccess)
            return Ok(result.Data);
        return StatusCode(result.StatusCode, new MessageDto(result.Message));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(MessageDto))]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] LoginDto dto)
    {
        var result = await _loginService.Register(dto);
        if (result.IsSuccess)
            return Ok();
        return StatusCode(result.StatusCode, new MessageDto(result.Message));
    }

    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserExpandedDto))]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUser(int id)
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
        return Ok(new UserExpandedDto() { Username = user.Username, ReviewAlbumPairs = reviewAlbumPairs });
    }
}