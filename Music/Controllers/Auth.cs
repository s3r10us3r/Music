using Microsoft.AspNetCore.Mvc;
using Music.Dtos;
using Music.Services;

namespace Music.Controllers;

public class Auth : Controller
{
    private readonly LoginService _loginService;
    
    public Auth(LoginService loginService)
    {
        _loginService = loginService;
    }
    
    public IActionResult Login()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(LoginDto model)
    {
        var response = await _loginService.Register(model);
        if (response.IsSuccess)
            return RedirectToAction("Login");
        ModelState.AddModelError("Username", response.Message);
        return View(model);
    }
}