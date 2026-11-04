using DummyJson.Tests.Api;
using DummyJson.Tests.Api.Model;
using DummyJson.Tests.Api.Request;
using DummyJson.Tests.Api.Response;
using DummyJson.Tests.Support;
using DummyJson.Tests.Tables;
using Reqnroll;

namespace DummyJson.Tests.StepDefinitions;

[Binding]
public sealed class CartSteps(IDummyJsonApi api, ApiContext context)
{
    [When("I add the following products to a new cart for user {int}")]
    public async Task WhenIAddTheFollowingProductsToANewCartForUser(int userId, IEnumerable<CartProductTable> rows)
    {
        var request = new AddCartRequest
        {
            UserId = userId,
            Products = rows
                .Select(r => new CartProduct { Id = r.ProductId, Quantity = r.Quantity })
                .ToList()
        };

        context.LastRequestBody = request;
        context.LastResponse = await api.AddCartAsync(request);
    }

    [Then("the cart should contain {int} distinct products with a total quantity of {int}")]
    public void ThenTheCartShouldContainDistinctProductsWithATotalQuantityOf(int products, int quantity)
    {
        var cart = context.Content<CartResponse>();
        Assert.Multiple(() =>
        {
            Assert.That(cart.Products, Has.Count.EqualTo(products));
            Assert.That(cart.TotalProducts, Is.EqualTo(products));
            Assert.That(cart.TotalQuantity, Is.EqualTo(quantity));
        });
    }

    [Then("each cart line should match the requested product and quantity")]
    public void ThenEachCartLineShouldMatchTheRequestedProductAndQuantity()
    {
        var request = (AddCartRequest)context.LastRequestBody!;
        var cart = context.Content<CartResponse>();

        Assert.Multiple(() =>
        {
            Assert.That(cart.UserId, Is.EqualTo(request.UserId));
            Assert.That(cart.Products, Has.Count.EqualTo(request.Products.Count));

            foreach (var requested in request.Products)
            {
                var line = cart.Products.SingleOrDefault(p => p.Id == requested.Id);
                Assert.That(line?.Quantity, Is.EqualTo(requested.Quantity), $"Quantity for product {requested.Id}");
            }
        });
    }

    [Then("the total discount percentage of the cart should be {decimal}")]
    public void ThenTheTotalDiscountPercentageOfTheCartShouldBe(decimal expected)
    {
        var cart = context.Content<CartResponse>();
        var totalDiscountPercentage = cart.Products.Sum(p => p.DiscountPercentage);

        TestContext.Out.WriteLine($"Total discount percentage: {totalDiscountPercentage}");
        Assert.That(totalDiscountPercentage, Is.EqualTo(expected));
    }

    [Then("the cart totals should be consistent with its lines")]
    public void ThenTheCartTotalsShouldBeConsistentWithItsLines()
    {
        var cart = context.Content<CartResponse>();

        Assert.Multiple(() =>
        {
            foreach (var line in cart.Products)
            {
                Assert.That(line.Total, Is.EqualTo(line.Price * line.Quantity).Within(0.01m),
                    $"Line total for product {line.Id}");
            }

            Assert.That(cart.Total, Is.EqualTo(cart.Products.Sum(p => p.Total)).Within(0.01m));
            Assert.That(cart.DiscountedTotal, Is.LessThan(cart.Total));
        });
    }

    [When("I delete the cart with id {int}")]
    public async Task WhenIDeleteTheCartWithId(int id)
    {
        context.LastResponse = await api.DeleteCartAsync(id);
    }

    [Then("the response should contain cart id {int}")]
    public void ThenTheResponseShouldContainCartId(int id)
    {
        var cart = context.Content<CartResponse>();
        Assert.That(cart.Id, Is.EqualTo(id));
    }

    [Then("the cart should be flagged as deleted with a deletion timestamp")]
    public void ThenTheCartShouldBeFlaggedAsDeletedWithADeletionTimestamp()
    {
        var cart = context.Content<CartResponse>();
        Assert.Multiple(() =>
        {
            Assert.That(cart.IsDeleted, Is.True);
            Assert.That(cart.DeletedOn, Is.Not.Null);
            Assert.That(cart.DeletedOn, Is.EqualTo(DateTimeOffset.UtcNow).Within(TimeSpan.FromMinutes(5)),
                "deletedOn should be roughly now");
        });
    }
}