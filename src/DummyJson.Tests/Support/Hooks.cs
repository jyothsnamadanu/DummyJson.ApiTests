using DummyJson.Tests.Api;
using Refit;
using Reqnroll;
using Reqnroll.BoDi;

namespace DummyJson.Tests.Support;

[Binding]
public sealed class Hooks
{
    private static TestSettings _settings = null!;
    private static HttpClient _httpClient = null!;
    private static IDummyJsonApi _api = null!;

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        _settings = TestSettings.Load();
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_settings.BaseUrl.TrimEnd('/')),
            Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds)
        };
        _api = RestService.For<IDummyJsonApi>(_httpClient);
    }

    [AfterTestRun]
    public static void AfterTestRun() => _httpClient.Dispose();

    [BeforeScenario(Order = 0)]
    public void RegisterDependencies(IObjectContainer container)
    {
        container.RegisterInstanceAs(_settings);
        container.RegisterInstanceAs(_api);
    }
}
