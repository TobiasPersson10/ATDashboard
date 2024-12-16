using System.Text.Json.Serialization;

namespace ATDashboard.Models.Requests;

public class InvoiceRequest
{
    [JsonPropertyName("status")]
    public int status { get; set; }

    [JsonPropertyName("DST")]
    public string DST { get; set; }

    [JsonPropertyName("months")]
    public int months { get; set; }

    [JsonPropertyName("source")]
    public string source { get; set; }

    [JsonPropertyName("customerId")]
    public string customerId { get; set; }

    [JsonPropertyName("utility")]
    public string utility { get; set; }

    [JsonPropertyName("subscriptionNr")]
    public int subscriptionNr { get; set; }

    [JsonPropertyName("maxHitOnPage")]
    public int maxHitOnPage { get; set; }

    [JsonPropertyName("pageNumber")]
    public int pageNumber { get; set; }
}
