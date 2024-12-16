using System.Text.Json;
using ATDashboard.Models;
using ATDashboard.Models.Requests;
using ATDashboard.Models.SkeKraftModels;
using Microsoft.Extensions.Caching.Memory;

namespace ATDashboard.Services;

public class CustomerService(ISkeKraftClient client, IMemoryCache cache, ILogger<CustomerService> logger) : ICustomerService
{
    private const string GetCustomerInfoUri = "GetCustomerInfo";
    private const string GetInvoicesWithPageingUri = "GetInvoicesWithPageing";

    public async Task<CustomerInfoResponse?> GetCustomerInfo(CustomerInfoRequest request, CancellationToken cancellationToken = default)
    {
        if (cache.TryGetValue(request.DST, out CustomerInfoResponse? customerInfoResponse))
            return customerInfoResponse;

        HttpResponseMessage response;
        try
        {
            response = await client.GetCustomerInfoAsync(GetCustomerInfoUri, request, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to fetch data for CustomerInfoAsync");
            return null;
        }

        try
        {
            var customerInfo = await response.Content.ReadFromJsonAsync<CustomerInfoResponse>(cancellationToken: cancellationToken);
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

    public async Task<InvoiceResponse?> GetInvoice(InvoiceRequest request, CancellationToken cancellationToken = default)
    {
        if (cache.TryGetValue(request.DST, out InvoiceResponse? invoiceResponse))
            return invoiceResponse;

        HttpResponseMessage response;
        try
        {
            response = await client.GetInvoiceAsync(GetInvoicesWithPageingUri, request, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            logger.LogError(e, "failed to fetch data for GetInvoice");
            return null;
        }

        try
        {
            var invoice = await response.Content.ReadFromJsonAsync<InvoiceResponse>(cancellationToken: cancellationToken);
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
    Task<CustomerInfoResponse?> GetCustomerInfo(CustomerInfoRequest request, CancellationToken cancellationToken = default);
    Task<InvoiceResponse?> GetInvoice(InvoiceRequest request, CancellationToken cancellationToken = default);
}
