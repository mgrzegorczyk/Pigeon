using Microsoft.AspNetCore.Mvc;
using Pigeon.Application.DTOs;
using Pigeon.Application.Services;

namespace Pigeon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost]
    [Route(nameof(Register))]
    public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
    {
        await _authService.RegisterUser(registerUserDto);

        return Created();
    }
}