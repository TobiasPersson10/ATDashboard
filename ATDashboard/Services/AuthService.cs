using System.Text;
using System.Text.Json;
using ATDashboard.Models.Requests;

namespace ATDashboard.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task LoginAsync(CancellationToken cancellationToken = default)
    {
        var loginRequest = new LoginRequest
        {
            username = "***",
            password = "***"
        };

        try
        {
            var content = SerializeToJsonContent(loginRequest);
            var response = await _httpClient.PostAsync("Login", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                await LogErrorAsync(response);
            }

            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            // Log or rethrow depending on error handling strategy
            Console.WriteLine($"An exception occurred: {ex.Message}");
            throw;
        }
    }

    private static StringContent SerializeToJsonContent<T>(T data)
    {
        var json = JsonSerializer.Serialize(data);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private static async Task LogErrorAsync(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error: {response.StatusCode}");
        Console.WriteLine($"Response Content: {responseContent}");
    }
}

public interface IAuthService
{
    Task LoginAsync(CancellationToken ct = default);
}