using System.Text;
using System.Text.Json;
using ATDashboard.Models.Requests;

namespace ATDashboard;

public class SkeKraftClient(HttpClient httpClient) : ISkeKraftClient
{
    public Task<HttpResponseMessage> GetCustomerInfoAsync(string uri, CustomerInfoRequest request, CancellationToken cancellationToken = default)
    {
        return httpClient.PostAsJsonAsync(uri, request, cancellationToken);
    }

    public Task<HttpResponseMessage> LoginAsync(string uri, LoginRequest request, CancellationToken cancellationToken = default)
    {
        return httpClient.PostAsJsonAsync(uri, request, cancellationToken);
    }

    public Task<HttpResponseMessage> GetInvoiceAsync(string uri, InvoiceRequest request, CancellationToken cancellationToken = default)
    {
        return httpClient.PostAsJsonAsync(uri, request, cancellationToken);
    }
}

public interface ISkeKraftClient
{
    Task<HttpResponseMessage> GetCustomerInfoAsync(string uri, CustomerInfoRequest request, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> LoginAsync(string uri, LoginRequest request, CancellationToken cancellationToken = default);
    Task<HttpResponseMessage> GetInvoiceAsync(string uri, InvoiceRequest request, CancellationToken cancellationToken = default);
}
