#pragma warning disable CS8618
using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Tests.Fixtures;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public abstract class TestClass<TFixture> : IClassFixture<TFixture> where TFixture : class, ITestFixture
{
    protected TFixture Fixture { get; init; }
    protected ITestOutputHelper Output { get; init; }
    protected Faker Fake => Fixture.Fake;

    protected TestClass(TFixture f, ITestOutputHelper o)
    {
        Fixture = f;
        Output = o;
    }
}

public interface ITestFixture
{
    Faker Fake { get; }
}

public abstract class TestFixture<TProgram> : IAsyncLifetime, ITestFixture where TProgram : class
{
    public Faker Fake => _faker;
    public IServiceProvider Services => _app.Services;
    public TestServer Server => _app.Server;
    public HttpClient Client { get; set; }

    private static readonly Faker _faker = new();
    private static WebApplicationFactory<TProgram> _app;

    private readonly IMessageSink _messageSink;

    static TestFixture()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
    }

    protected TestFixture(IMessageSink s)
    {
        _messageSink = s;
        _app ??= new WebApplicationFactory<TProgram>().WithWebHostBuilder(b =>
        {
            b.ConfigureLogging(l => l.ClearProviders().AddXUnit(_messageSink));
            ConfigureApp(b);
            b.ConfigureTestServices(ConfigureServices);
        });
        Client = _app.CreateClient();
    }

    protected virtual Task SetupAsync() => Task.CompletedTask;
    protected virtual Task TearDownAsync() => Task.CompletedTask;
    protected virtual void ConfigureApp(IWebHostBuilder a) { }
    protected virtual void ConfigureServices(IServiceCollection s) { }

    public HttpClient CreateClient(WebApplicationFactoryClientOptions? o = null) => CreateClient(_ => { }, o);

    public HttpClient CreateClient(Action<HttpClient> c, WebApplicationFactoryClientOptions? o = null)
    {
        var client = o is null ? _app.CreateClient() : _app.CreateClient(o);
        c(client);
        return client;
    }

    public HttpMessageHandler CreateHandler(Action<HttpContext>? c = null)
        => c is null ? _app.Server.CreateHandler() : _app.Server.CreateHandler(c);

    public Task InitializeAsync() => SetupAsync();

    public virtual Task DisposeAsync()
    {
        Client.Dispose();
        return TearDownAsync();
    }
}