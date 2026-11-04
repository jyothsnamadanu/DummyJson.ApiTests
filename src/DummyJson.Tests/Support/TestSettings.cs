using Microsoft.Extensions.Configuration;

namespace DummyJson.Tests.Support;

public sealed class TestSettings
{
    public string BaseUrl { get; init; } = "https://dummyjson.com";
    public int TimeoutSeconds { get; init; } = 30;
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public int ExpiresInMins { get; init; } = 30;

    public static TestSettings Load()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        return new TestSettings
        {
            BaseUrl = config["Api:BaseUrl"] ?? "https://dummyjson.com",
            TimeoutSeconds = int.TryParse(config["Api:TimeoutSeconds"], out var t) ? t : 30,
            Username = config["Auth:Username"] ?? string.Empty,
            Password = config["Auth:Password"] ?? string.Empty,
            ExpiresInMins = int.TryParse(config["Auth:ExpiresInMins"], out var e) ? e : 30
        };
    }
}
