using System.Net;
using DummyJson.Tests.Support;
using Reqnroll;

namespace DummyJson.Tests.StepDefinitions;

[Binding]
public sealed class CommonSteps(ApiContext context)
{
    [Then("the response status code should be {int}")]
    public void ThenTheResponseStatusCodeShouldBe(int expected)
    {
        Assert.That(context.LastResponse.StatusCode, Is.EqualTo((HttpStatusCode)expected),
            $"Unexpected status code. Body: {context.LastResponse.Error?.Content}");
        Assert.That(context.LastResponse.ContentHeaders?.ContentType?.MediaType, Is.EqualTo("application/json"));
    }
}
