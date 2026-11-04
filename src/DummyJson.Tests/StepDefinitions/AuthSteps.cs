using System.Net;
using DummyJson.Tests.Api;
using DummyJson.Tests.Api.Request;
using DummyJson.Tests.Api.Response;
using DummyJson.Tests.Support;
using DummyJson.Tests.Tables;
using Reqnroll;

namespace DummyJson.Tests.StepDefinitions;

[Binding]
public sealed class AuthSteps(IDummyJsonApi api, ApiContext context, TestSettings settings)
{
    [When("I log in with the configured user credentials")]
    public async Task WhenILogInWithTheConfiguredUserCredentials()
    {
        context.LastResponse = await api.LoginAsync(new LoginRequest
        {
            Username = settings.Username,
            Password = settings.Password,
            ExpiresInMins = settings.ExpiresInMins
        });
    }

    [Then("the response should contain an access token and a refresh token")]
    public void ThenTheResponseShouldContainAnAccessTokenAndARefreshToken()
    {
        var login = context.Content<LoginResponse>();

        Assert.Multiple(() =>
        {
            Assert.That(login.Username, Is.EqualTo(settings.Username));
            Assert.That(login.AccessToken, Is.Not.Null.And.Not.Empty);
            Assert.That(login.RefreshToken, Is.Not.Null.And.Not.Empty);
            Assert.That(login.AccessToken.Split('.'), Has.Length.EqualTo(3), "Access token is not a JWT");
        });
    }

    [Then("the access token should be stored for subsequent requests")]
    public void ThenTheAccessTokenShouldBeStoredForSubsequentRequests()
    {
        var login = context.Content<LoginResponse>();
        context.AccessToken = login.AccessToken;

        Assert.That(context.AccessToken, Is.EqualTo(login.AccessToken));
    }

    [Given("I am authenticated as the configured user")]
    public async Task GivenIAmAuthenticatedAsTheConfiguredUser()
    {
        await WhenILogInWithTheConfiguredUserCredentials();
        Assert.That(context.LastResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Login failed");
        ThenTheAccessTokenShouldBeStoredForSubsequentRequests();
    }

    [When("I request the current authenticated user")]
    public async Task WhenIRequestTheCurrentAuthenticatedUser()
    {
        Assert.That(context.AccessToken, Is.Not.Null, "No access token stored; authenticate first.");
        context.LastResponse = await api.GetCurrentUserAsync(context.AccessToken!);
    }

    [Then("the authenticated user should have the following details")]
    public void ThenTheAuthenticatedUserShouldHaveTheFollowingDetails(UserTable expectedUser)
    {
        var actualUser = context.Content<UserResponse>();

        Assert.Multiple(() =>
        {
            Assert.That(actualUser.Id, Is.EqualTo(expectedUser.Id));
            Assert.That(actualUser.FirstName, Is.EqualTo(expectedUser.FirstName));
            Assert.That(actualUser.LastName, Is.EqualTo(expectedUser.LastName));
        });
    }
}
