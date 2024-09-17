using HRMSPOC.API.DTOs;
using HRMSPOC.API.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMSPOC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthDTO registerDto)
        {
            var token = await _authService.RegisterAsync(registerDto);
            return Ok(new {token});
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthDTO loginDto)
        {
            var token = await _authService.LoginAsync(loginDto);
            return Ok(new {token});
        }
    }
}
