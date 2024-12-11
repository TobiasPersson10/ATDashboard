using System.Text;
using System.Text.Json;
using ATDashboard.Configuration;
using ATDashboard.Models;
using ATDashboard.Models.Requests;
using Microsoft.Extensions.Options;

namespace ATDashboard.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ExternalApiSettings _externalApiSettings;

    public AuthService(HttpClient httpClient, IOptions<ExternalApiSettings> options)
    {
        _httpClient = httpClient;
        _externalApiSettings = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<LoginResponse?> LoginAsync(CancellationToken cancellationToken = default)
    {
        var loginRequest = new LoginRequest
        {
            username = _externalApiSettings.UserName,
            password = _externalApiSettings.Password,
        };

        try
        {
            var requestJson = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Login", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                await LogErrorAsync(response);
                //throw error
            }

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(json);
            return loginResponse;
        }
        catch (Exception ex)
        {
            // Log or rethrow depending on error handling strategy
            Console.WriteLine($"An exception occurred: {ex.Message}");
            throw;
        }
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
    Task<LoginResponse?> LoginAsync(CancellationToken ct = default);
}
