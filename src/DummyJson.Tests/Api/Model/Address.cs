using System.Text.Json.Serialization;

namespace DummyJson.Tests.Api.Model;

public sealed class Address
{
    [JsonPropertyName("address")]
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string StateCode { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public Coordinates Coordinates { get; set; } = new();
    public string Country { get; set; } = string.Empty;
}
