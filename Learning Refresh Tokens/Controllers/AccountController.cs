using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Refresh.Application.Dtos;
using Refresh.Application.Dtos.RefreshToken;
using Refresh.Application.Interfaces;

namespace Learning_Refresh_Tokens.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : Controller
{
    private readonly IJwtAuthenticationService _authenticationService;

    public AccountController(IJwtAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto req)
    {
        var result = await _authenticationService.Login(req);
        return result is not null ? result : Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost("Refresh")]
    public async Task<ActionResult<LoginResponseDto>> Refresh([FromBody] RefreshRequestDto req)
    {
        if (string.IsNullOrWhiteSpace(req.Token))
            return BadRequest("Invalid Token");
        
        var result = await _authenticationService.ValidateRefreshToken(req.Token);
        return result is not null ? result : Unauthorized();
    }
}