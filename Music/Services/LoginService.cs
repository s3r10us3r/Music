using System.Net;
using Dal.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Music.Dtos;
using Music.Services.Interfaces;

namespace Music.Services;

public class LoginService
{
    private readonly IUserRepo _userRepo;
    private readonly IPasswordHasher<object> _passwordHasher;
    private readonly IPasswordCheckingService _passwordCheckingService;
    private readonly IJwtService _jwtService;

    public LoginService(IUserRepo userRepo, IPasswordHasher<object> passwordHasher,
        IPasswordCheckingService passwordCheckingService, IJwtService jwtService)
    {
        _userRepo = userRepo;
        _passwordHasher = passwordHasher;
        _passwordCheckingService = passwordCheckingService;
        _jwtService = jwtService;
    }

    public async Task<IActionResult> Register(LoginDto dto)
    {
        if (dto.Username.Length < 6)
            return new BadRequestObjectResult(new MessageDto("Username is too short!"));
        if (!_passwordCheckingService.CheckPassword(dto.Password, out var message))
            return new BadRequestObjectResult(new MessageDto(message));
        var existingUser = await _userRepo.FindAsync(u => u.Username == dto.Username);
        if (existingUser != null)
            return new ConflictResult();
        var user = new User()
        {
            Username = dto.Username,
            HashedPassword = _passwordHasher.HashPassword(dto, dto.Password)
        };
        var model = await _userRepo.CreateAsync(user);
        return new OkResult();
    }

    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userRepo.FindAsync(u => u.Username == dto.Username);
        if (user == null)
            return new UnauthorizedObjectResult(new MessageDto("Invalid credentials"));
        var passwordValidationResult = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, dto.Password);
        if (passwordValidationResult != PasswordVerificationResult.Success)
            return new UnauthorizedObjectResult(new MessageDto("Invalid credentials"));
        var jwt = _jwtService.CreateJwt(user);
        return new OkObjectResult(new LoginResponseDto(jwt));
    }
}