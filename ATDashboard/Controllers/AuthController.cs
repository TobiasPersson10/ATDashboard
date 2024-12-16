using ATDashboard.Models.DTO;
using ATDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace ATDashboard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public async Task<IActionResult> Login(CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(cancellationToken);
        if (result is null)
            return NoContent();

        return Ok(LoginResponseDto.ToLoginResponseDto(result));
    }
}
