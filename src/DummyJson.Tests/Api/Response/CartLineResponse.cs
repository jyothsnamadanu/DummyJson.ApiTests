namespace DummyJson.Tests.Api.Response;

public sealed class CartLineResponse
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
    public decimal DiscountPercentage { get; set; }
}
