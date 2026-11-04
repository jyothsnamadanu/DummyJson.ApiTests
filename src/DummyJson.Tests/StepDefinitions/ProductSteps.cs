using System.Text.Json;
using System.Text.Json.Nodes;
using DummyJson.Tests.Api;
using DummyJson.Tests.Api.Request;
using DummyJson.Tests.Api.Response;
using DummyJson.Tests.Support;
using DummyJson.Tests.Tables;
using Reqnroll;

namespace DummyJson.Tests.StepDefinitions;

[Binding]
public sealed class ProductSteps(IDummyJsonApi api, ApiContext context)
{
    [When("I search for products matching {string}")]
    public async Task WhenISearchForProductsMatching(string query)
    {
        context.LastResponse = await api.SearchProductsAsync(query);
    }

    [Then("the search results should contain exactly one product titled {string}")]
    public void ThenTheSearchResultsShouldContainExactlyOneProductTitled(string title)
    {
        var products = context.Content<ProductListResponse>();
        Assert.That(products.Products.Count(p => p.Title == title), Is.EqualTo(1));
    }

    [Then("the product {string} should have a price of {decimal}")]
    public void ThenTheProductShouldHaveAPriceOf(string title, decimal price)
    {
        var product = context.Content<ProductListResponse>().Products.Single(p => p.Title == title);
        Assert.That(product.Price, Is.EqualTo(price));
    }

    [When("I request page {int} of the products list with a page size of {int}, selecting only {string}")]
    public async Task WhenIRequestPageOfTheProductsList(int page, int pageSize, string fields)
    {
        var skip = (page - 1) * pageSize;
        context.LastResponse = await api.GetProductsAsync(pageSize, skip, fields);
    }

    [Then("the response should contain only {int} product(s)")]
    public void ThenTheResponseShouldContainOnlyProducts(int expected)
    {
        Assert.That(PagedProducts().Products, Has.Count.EqualTo(expected));
    }

    [Then("the returned product should have the following details")]
    public void ThenTheReturnedProductShouldHaveTheFollowingDetails(ProductTable expected)
    {
        var actual = PagedProducts().Products.Single();

        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Title, Is.EqualTo(expected.Title));
            Assert.That(actual.Price, Is.EqualTo(expected.Price));
        });
    }

    [Then("the returned product should only contain the fields {string}")]
    public void ThenTheReturnedProductShouldOnlyContainTheFields(string fields)
    {
        var product = context.Content<JsonObject>()["products"]!.AsArray().Single()!.AsObject();
        var expected = fields.Split(',', StringSplitOptions.TrimEntries);
        Assert.That(product.Select(p => p.Key), Is.EquivalentTo(expected));
    }

    [When("I update the title of product {int} to {string}")]
    public async Task WhenIUpdateTheTitleOfProductTo(int id, string title)
    {
        context.LastResponse = await api.UpdateProductAsync(id, new UpdateProductRequest { Title = title });
    }

    [Then("the response should contain product id {int}")]
    public void ThenTheResponseShouldContainProductId(int id)
    {
        Assert.That(context.Content<ProductResponse>().Id, Is.EqualTo(id));
    }

    [Then("the product title in the response should be {string}")]
    public void ThenTheProductTitleInTheResponseShouldBe(string title)
    {
        Assert.That(context.Content<ProductResponse>().Title, Is.EqualTo(title));
    }

    private ProductListResponse PagedProducts() =>
        context.Content<JsonObject>().Deserialize<ProductListResponse>(JsonSerializerOptions.Web)!;
}
