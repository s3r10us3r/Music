using System.Net;
using Dal.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Music.Dtos;
using Music.Helpers;
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

    public async Task<Result> Register(LoginDto dto)
    {
        if (dto.Username.Length < 6)
            return Result.Failure(404, "Username is too short!");
        if (!_passwordCheckingService.CheckPassword(dto.Password, out var message))
            return Result.Failure(404, message);
        var existingUser = await _userRepo.FindAsync(u => u.Username == dto.Username);
        if (existingUser != null)
            return Result.Failure(409, "Username already exists");
        var user = new User()
        {
            Username = dto.Username,
            HashedPassword = _passwordHasher.HashPassword(dto, dto.Password)
        };
        var model = await _userRepo.CreateAsync(user);
        return Result.Success(200);
    }

    public async Task<Result<LoginResponseDto>> Login(LoginDto dto)
    {
        var user = await _userRepo.FindAsync(u => u.Username == dto.Username);
        if (user == null)
            return Result<LoginResponseDto>.Failure(401, "Invalid credentials");
        var passwordValidationResult = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, dto.Password);
        if (passwordValidationResult != PasswordVerificationResult.Success)
            return Result<LoginResponseDto>.Failure(401, "Invalid credentials");
        var jwt = _jwtService.CreateJwt(user);
        return Result<LoginResponseDto>.Success(200, new LoginResponseDto(jwt));
    }
}