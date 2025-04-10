using API.SERVICE.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedBlocks.Dtos.Request;

namespace API.PL.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDTO user)
    {
        return Ok(await _authService.Register(user));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDTO user)
    {
        var result = await _authService.Login(user);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenRequestDTO tokenRequest)
    {
        return Ok(await _authService.RefreshToken(tokenRequest));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("protected")]
    //[Authorize]
    public IActionResult Test()
    {
        return Ok("Protected Data !");
    }
}
