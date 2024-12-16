using System.Text.Json;
using ATDashboard.Models.DTO;
using ATDashboard.Models.Requests;
using ATDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace ATDashboard.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController(IAuthService authService, ICustomerService customerService) : ControllerBase
{
    [HttpGet("CustomerInfo")]
    public async Task<IActionResult> GetCustomerInfo(CancellationToken cancellationToken = default)
    {
        var token = authService.GetAccessToken();
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

        var customerInfo = await customerService.GetCustomerInfo(loginRequest, cancellationToken);
        if (customerInfo is null)
            return NotFound();

        var customerInfoDto = CustomerInfoDto.ToCustomerInfoDto(customerInfo);
        return Ok(customerInfoDto);
    }

    [HttpGet("Invoices")]
    public async Task<IActionResult> GetInvoices(CancellationToken cancellationToken = default)
    {
        var token = authService.GetAccessToken();
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

        var invoiceResponse = await customerService.GetInvoice(invoiceRequest, cancellationToken);
        if (invoiceResponse is null)
            return NotFound();

        var invoiceDtoList = invoiceResponse.InvoiceInfo?.Select(InvoiceDto.ToInvoiceDto).ToList();
        return Ok(invoiceDtoList);
    }
}

public class RootObject
{
    public string DST { get; set; }
    public string customerId { get; set; }
    public string source { get; set; }
}
