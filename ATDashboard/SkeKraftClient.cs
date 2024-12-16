using System.Text;
using System.Text.Json;
using ATDashboard.Models.Requests;

namespace ATDashboard;

public class SkeKraftClient(HttpClient httpClient)
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
        // var json = JsonSerializer.Serialize(request);
        // var content = new StringContent(json, Encoding.UTF8, "application/json");
        // var res = httpClient.PostAsync(uri, content, cancellationToken);
        // return res;
        return httpClient.PostAsJsonAsync(uri, request, cancellationToken);
    }
}
