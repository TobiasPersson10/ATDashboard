using ATDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace ATDashboard.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IAuthService _authService;

    public CustomerController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> GetCustomer()
    {
        await _authService.LoginAsync();
        return Ok("Login successful");
    }
}