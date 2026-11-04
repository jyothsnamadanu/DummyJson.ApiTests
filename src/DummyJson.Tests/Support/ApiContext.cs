using Refit;

namespace DummyJson.Tests.Support;

public sealed class ApiContext
{
    private IApiResponse? _lastResponse;

    public IApiResponse LastResponse
    {
        get => _lastResponse ?? throw new InvalidOperationException("No request has been sent in this scenario yet.");
        set => _lastResponse = value;
    }

    public string? AccessToken { get; set; }

    public object? LastRequestBody { get; set; }

    public T Content<T>() =>
        LastResponse is IApiResponse<T> { Content: { } content }
            ? content
            : throw new InvalidOperationException(
                $"Last response has no {typeof(T).Name} content (status {(int)LastResponse.StatusCode}): {LastResponse.Error?.Content}");
}
