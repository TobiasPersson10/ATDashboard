using ATDashboard.Models;
using ATDashboard.Models.DTO;
using ATDashboard.Models.Requests;
using ATDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace ATDashboard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ICustomerService _customerService;

    public CustomerController(IAuthService authService, ICustomerService customerService)
    {
        _authService = authService;
        _customerService = customerService;
    }

    [HttpPost]
    public async Task<IActionResult> Get([FromBody] CustomerInfoRequest customerInfoRequest)
    {
        // Check if logged in
        var token = _authService.GetAccessToken();
        if (token is null)
        {
            return Unauthorized("Token is not loaded, attempt a login");
        }

        var customerInfo = await _customerService.GetCustomerInfo(customerInfoRequest);
        if (customerInfo is null)
            return NotFound();

        var dto = CustomerInfoDTO.ToDomain(customerInfo);
        return Ok(dto);
        // Call customerService
    }
}
