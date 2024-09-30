using HRMSPOC.API.DTOs;
using HRMSPOC.API.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HRMSPOC.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthDTO loginDto)
    {
        var token = await _authService.LoginAsync(loginDto);
        return Ok(new {token});
    }
}
