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
    
    public async Task LoginAsync()
    {
        var body = new LoginRequest
        {
            password = "***",
            username = "***"
        };

        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
    
        var response = await _httpClient.PostAsync("Login", content);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            // Log the response status code and content for debugging
            Console.WriteLine($"Error: {response.StatusCode}");
            Console.WriteLine($"Response Content: {responseContent}");
        }
        response.EnsureSuccessStatusCode();
    }
}

public interface IAuthService
{
    Task LoginAsync();
}