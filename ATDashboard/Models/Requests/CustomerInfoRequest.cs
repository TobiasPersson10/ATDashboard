using System.Text.Json.Serialization;

namespace ATDashboard.Models.Requests;

public class CustomerInfoRequest
{
    [JsonPropertyName("DST")]
    public string DST { get; set; }

    [JsonPropertyName("customerId")]
    public string customerId { get; set; }

    [JsonPropertyName("source")]
    public string source { get; set; }
}
