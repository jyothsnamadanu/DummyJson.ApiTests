using DummyJson.Tests.Api.Model;

namespace DummyJson.Tests.Api.Response;

public sealed class UserResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
    public Hair? Hair { get; set; }
    public Address? Address { get; set; }
}
