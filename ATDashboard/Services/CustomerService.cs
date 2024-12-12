using System.Text.Json;
using ATDashboard.Models;
using ATDashboard.Models.Requests;
using Microsoft.Extensions.Caching.Memory;

namespace ATDashboard.Services;

public class CustomerService : ICustomerService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public CustomerService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<CustomerInfo?> GetCustomerInfo(CustomerInfoRequest customerInfoRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("Customer", customerInfoRequest);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var customerInfo = JsonSerializer.Deserialize<CustomerInfo>(json);
        if (customerInfo != null)
        {
            _cache.Set(customerInfoRequest.DST, customerInfo);
        }

        return customerInfo;
    }
}

public interface ICustomerService
{
    Task<CustomerInfo?> GetCustomerInfo(CustomerInfoRequest customerInfoRequest);
}
