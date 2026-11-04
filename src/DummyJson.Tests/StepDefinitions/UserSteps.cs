using DummyJson.Tests.Api;
using DummyJson.Tests.Api.Model;
using DummyJson.Tests.Api.Request;
using DummyJson.Tests.Api.Response;
using DummyJson.Tests.Support;
using DummyJson.Tests.Tables;
using Reqnroll;

namespace DummyJson.Tests.StepDefinitions;

[Binding]
public sealed class UserSteps(IDummyJsonApi api, ApiContext context)
{
    private const int SeededUserCount = 208;

    [When("I filter all users by {string} with value {string}")]
    public async Task WhenIFilterAllUsersBy(string key, string value)
    {
        context.LastResponse = await api.FilterUsersAsync(key, value, limit: 0);
    }

    [Then("the response should contain {int} users")]
    public void ThenTheResponseShouldContainUsers(int expected)
    {
        Assert.That(context.Content<UserListResponse>().Users, Has.Count.EqualTo(expected));
    }

    [Then("the total reported by the response should be {int}")]
    public void ThenTheTotalReportedByTheResponseShouldBe(int expected)
    {
        Assert.That(context.Content<UserListResponse>().Total, Is.EqualTo(expected));
    }

    [Then("every returned user should have hair colour {string}")]
    public void ThenEveryReturnedUserShouldHaveHairColour(string expectedColour)
    {
        var actualUsers = context.Content<UserListResponse>();
        Assert.Multiple(() =>
        {
            foreach (var user in actualUsers.Users)
            {
                Assert.That(user.Hair?.Color, Is.EqualTo(expectedColour), $"Hair colour for user {user.Id}");
            }
        });
    }

    [When("I add a new user with the following details")]
    public async Task WhenIAddANewUserWithTheFollowingDetails(NewUserTable row)
    {
        var request = new AddUserRequest
        {
            FirstName = row.FirstName,
            LastName = row.LastName,
            Age = row.Age,
            Gender = row.Gender,
            Hair = new Hair { Color = row.HairColour, Type = row.HairType },
            Address = new Address
            {
                Street = row.Address,
                City = row.City,
                State = row.State,
                StateCode = row.StateCode,
                PostalCode = row.PostalCode,
                Coordinates = new Coordinates { Lat = row.Latitude, Lng = row.Longitude },
                Country = row.Country
            }
        };

        context.LastRequestBody = request;
        context.LastResponse = await api.AddUserAsync(request);
    }

    [Then("the response should contain a newly generated user id")]
    public void ThenTheResponseShouldContainANewlyGeneratedUserId()
    {
        Assert.That(context.Content<UserResponse>().Id, Is.GreaterThan(SeededUserCount));
    }

    [Then("the created user in the response should match the request")]
    public void ThenTheCreatedUserInTheResponseShouldMatchTheRequest()
    {
        var expectedUser = (AddUserRequest)context.LastRequestBody!;
        var actualUser = context.Content<UserResponse>();

        Assert.Multiple(() =>
        {
            Assert.That(actualUser.FirstName, Is.EqualTo(expectedUser.FirstName));
            Assert.That(actualUser.LastName, Is.EqualTo(expectedUser.LastName));
            Assert.That(actualUser.Age, Is.EqualTo(expectedUser.Age));
            Assert.That(actualUser.Gender, Is.EqualTo(expectedUser.Gender));
            Assert.That(actualUser.Hair, Is.EqualTo(expectedUser.Hair).UsingPropertiesComparer());
            Assert.That(actualUser.Address, Is.EqualTo(expectedUser.Address).UsingPropertiesComparer());
        });
    }
}
