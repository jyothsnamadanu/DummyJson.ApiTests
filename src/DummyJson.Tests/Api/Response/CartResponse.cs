namespace DummyJson.Tests.Api.Response;

public sealed class CartResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<CartLineResponse> Products { get; set; } = [];
    public decimal Total { get; set; }
    public decimal DiscountedTotal { get; set; }
    public int TotalProducts { get; set; }
    public int TotalQuantity { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }
}
