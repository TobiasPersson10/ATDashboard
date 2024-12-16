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
        var cached = _cache.TryGetValue(request.DST, out CustomerInfoResponse? customerInfoResponse);
        if (cached)
            return customerInfoResponse;

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
        var cached = _cache.TryGetValue(request.DST, out InvoiceResponse? invoiceResponse);
        if (cached)
            return invoiceResponse;

        var response = await _client.GetInvoiceAsync("GetInvoicesWithPageing", request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var invoice = JsonSerializer.Deserialize<InvoiceResponse>(json);

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
