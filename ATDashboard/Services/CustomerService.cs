using System.Text.Json;
using ATDashboard.Models;
using ATDashboard.Models.Requests;
using ATDashboard.Models.SkeKraftModels;
using Microsoft.Extensions.Caching.Memory;

namespace ATDashboard.Services;

public class CustomerService(SkeKraftClient client, IMemoryCache cache, ILogger<CustomerService> logger) : ICustomerService
{
    public async Task<CustomerInfoResponse?> GetCustomerInfo(CustomerInfoRequest request)
    {
        if (cache.TryGetValue(request.DST, out CustomerInfoResponse? customerInfoResponse))
            return customerInfoResponse;

        HttpResponseMessage response;
        try
        {
            response = await client.GetCustomerInfoAsync("GetCustomerInfo", request);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to fetch data for CustomerInfoAsync");
            return null;
        }

        try
        {
            var customerInfo = await response.Content.ReadFromJsonAsync<CustomerInfoResponse>();
            if (customerInfo != null)
                cache.Set(request.DST, customerInfo);
            return customerInfo;
        }
        catch (Exception e)
        {
            logger.LogError(e, "JSON Deserialization failed for CustomerInfoAsync");
            return null;
        }
    }

    public async Task<InvoiceResponse?> GetInvoice(InvoiceRequest request)
    {
        if (cache.TryGetValue(request.DST, out InvoiceResponse? invoiceResponse))
            return invoiceResponse;

        HttpResponseMessage response;
        try
        {
            response = await client.GetInvoiceAsync("GetInvoicesWithPageing", request);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            logger.LogError(e, "failed to fetch data for GetInvoice");
            return null;
        }

        try
        {
            var invoice = await response.Content.ReadFromJsonAsync<InvoiceResponse>();
            if (invoice != null)
                cache.Set(request.DST, invoice);
            return invoice;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

public interface ICustomerService
{
    Task<CustomerInfoResponse?> GetCustomerInfo(CustomerInfoRequest request);
    Task<InvoiceResponse?> GetInvoice(InvoiceRequest request);
}
