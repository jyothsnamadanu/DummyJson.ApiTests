namespace DummyJson.Tests.Api.Response;

public sealed class ProductResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
