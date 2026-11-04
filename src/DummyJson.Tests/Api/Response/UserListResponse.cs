namespace DummyJson.Tests.Api.Response;

public sealed class UserListResponse
{
    public List<UserResponse> Users { get; set; } = [];
    public int Total { get; set; }
}
