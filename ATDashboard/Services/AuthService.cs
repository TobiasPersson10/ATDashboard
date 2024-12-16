using System.Text.Json;
using ATDashboard.Configuration;
using ATDashboard.Models;
using ATDashboard.Models.Requests;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ATDashboard.Services;

public class AuthService(SkeKraftClient client, IOptions<ExternalApiSettings> options, IMemoryCache cache, ILogger<AuthService> logger) : IAuthService
{
    private readonly ExternalApiSettings _externalApiSettings = options.Value ?? throw new ArgumentNullException(nameof(options));

    private const string TokenCacheKey = "AuthTokenDst";

    public async Task<LoginResponse?> LoginAsync(CancellationToken cancellationToken = default)
    {
        var loginRequest = new LoginRequest { username = _externalApiSettings.UserName, password = _externalApiSettings.Password };

        if (cache.TryGetValue(TokenCacheKey, out LoginResponse? data))
            return data;

        HttpResponseMessage response;
        try
        {
            response = await client.LoginAsync("Login", loginRequest, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch data for LoginAsync");
            return null;
        }

        try
        {
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken);
            if (loginResponse is not null)
                cache.Set(TokenCacheKey, loginResponse);
            return loginResponse;
        }
        catch (Exception e)
        {
            logger.LogError(e, "JSON Deserialization failed for LoginAsync");
            return null;
        }
    }

    public string? GetAccessToken()
    {
        cache.TryGetValue(TokenCacheKey, out LoginResponse? loginResponse);
        return loginResponse?.Dst;
    }
}

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(CancellationToken ct = default);
    string? GetAccessToken();
}
