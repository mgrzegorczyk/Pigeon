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
        try
        {
            await _authService.RegisterUser(registerUserDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest($"Internal error. Can't create user {registerUserDto.Username}");
        }

        return Created();
    }
}