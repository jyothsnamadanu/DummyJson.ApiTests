using DummyJson.Tests.Api.Model;

namespace DummyJson.Tests.Api.Request;

public sealed class AddUserRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
    public Hair Hair { get; set; } = new();
    public Address Address { get; set; } = new();
}
