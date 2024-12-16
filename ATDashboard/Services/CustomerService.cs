using System.Text.Json;
using ATDashboard.Models;
using ATDashboard.Models.Requests;
using ATDashboard.Models.SkeKraftModels;
using Microsoft.Extensions.Caching.Memory;

namespace ATDashboard.Services;

public class CustomerService : ICustomerService
{
    private readonly SkeKraftClient _client;
    private readonly IMemoryCache _cache;

    public CustomerService(SkeKraftClient client, IMemoryCache cache)
    {
        _client = client;
        _cache = cache;
    }

    public async Task<CustomerInfoResponse?> GetCustomerInfo(CustomerInfoRequest request)
    {
        var response = await _client.GetCustomerInfoAsync("GetCustomerInfo", request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var customerInfo = JsonSerializer.Deserialize<CustomerInfoResponse>(json);
        if (customerInfo != null)
        {
            _cache.Set(request.DST, customerInfo);
        }

        return customerInfo;
    }

    public async Task<InvoiceResponse?> GetInvoice(InvoiceRequest request)
    {
        var response = await _client.GetInvoiceAsync("GetInvoicesWithPageing", request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(
                System.Text.Unicode.UnicodeRanges.All
            ),
        };

        var invoice = JsonSerializer.Deserialize<InvoiceResponse>(json, options);

        if (invoice != null)
            _cache.Set(request.DST, invoice);

        return invoice;
    }
}

public interface ICustomerService
{
    Task<CustomerInfoResponse?> GetCustomerInfo(CustomerInfoRequest request);
    Task<InvoiceResponse?> GetInvoice(InvoiceRequest request);
}
