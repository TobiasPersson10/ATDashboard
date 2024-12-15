using ATDashboard.Configuration;
using ATDashboard.Services;
using Microsoft.Extensions.Options;

namespace ATDashboard;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add configuration
        builder.Services.Configure<ExternalApiSettings>(
            builder.Configuration.GetSection("ExternalApi")
        );
        // Add services to the container.
        builder.Services.AddMemoryCache();
        builder.Services.AddHttpClient<SkeKraftClient>(
            (serviceprovider, client) =>
            {
                var externalApiSettings = serviceprovider
                    .GetRequiredService<IOptions<ExternalApiSettings>>()
                    .Value;
                if (string.IsNullOrEmpty(externalApiSettings.BaseUrl))
                {
                    throw new InvalidOperationException("External API BaseUrl is not configured.");
                }

                client.BaseAddress = new Uri(externalApiSettings.BaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }
        );

        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
