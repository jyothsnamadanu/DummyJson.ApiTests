using DummyJson.Tests.Api.Model;

namespace DummyJson.Tests.Api.Request;

public sealed class AddCartRequest
{
    public int UserId { get; set; }
    public List<CartProduct> Products { get; set; } = [];
}
