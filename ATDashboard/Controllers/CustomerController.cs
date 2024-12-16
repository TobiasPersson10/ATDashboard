using System.Text.Json;
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

    [HttpGet("CustomerInfo")]
    public async Task<IActionResult> GetCustomerInfo()
    {
        // Check if logged in
        var token = _authService.GetAccessToken();
        if (token is null)
        {
            return Unauthorized("Token is not loaded, attempt a login");
        }

        var loginRequest = new CustomerInfoRequest()
        {
            DST = token,
            customerId = "",
            source = "All",
        };

        var customerInfo = await _customerService.GetCustomerInfo(loginRequest);
        if (customerInfo is null)
            return NotFound();

        var dto = CustomerInfoDto.ToCustomerInfoDto(customerInfo);
        return Ok(dto);
    }

    [HttpGet("Invoices")]
    public async Task<IActionResult> GetInvoices()
    {
        var token = _authService.GetAccessToken();
        if (token is null)
        {
            return Unauthorized("Token in not loaded, attempt a login");
        }

        var invoiceRequest = new InvoiceRequest()
        {
            status = 0,
            DST = token,
            months = 18,
            source = "All",
            customerId = "",
            utility = "EL_EXT",
            subscriptionNr = 2581468,
            maxHitOnPage = 20,
            pageNumber = 1,
        };

        var invoiceResponse = await _customerService.GetInvoice(invoiceRequest);
        if (invoiceResponse is null)
            return NotFound();

        var invoiceDto = invoiceResponse.InvoiceInfo?.Select(InvoiceDto.ToInvoiceDto).ToList();

        var returnJson = JsonSerializer.Serialize(invoiceDto);

        return Ok(returnJson);
    }
}

public class RootObject
{
    public string DST { get; set; }
    public string customerId { get; set; }
    public string source { get; set; }
}
