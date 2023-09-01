﻿#pragma warning disable CS8618
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Tests.Fixtures;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public abstract class TestBase : IClassFixture<AppFixture>
{
    protected AppFixture App { get; init; }

    protected TestBase(AppFixture fixture)
    {
        App = fixture;
    }
}

public sealed class AppFixture : WebApplicationFactory<Program>
{
    public HttpClient GuestClient { get; private set; }
    public HttpClient AdminClient { get; private set; }
    public HttpClient AuthorClient { get; private set; }

    private readonly IMessageSink _messageSink;

    public AppFixture(IMessageSink messageSink)
    {
        _messageSink = messageSink;
        InitClients();
    }

    protected override void ConfigureWebHost(IWebHostBuilder b)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        b.ConfigureLogging(l => l.AddXUnit(_messageSink));
        b.ConfigureTestServices(s =>
        {

        });
    }

    private void InitClients()
    {
        GuestClient = CreateClient();
        AdminClient = CreateClient();
        AuthorClient = CreateClient();
    }

    public HttpMessageHandler CreateHttpMessageHandler()
        => Server.CreateHandler();

    public IServiceProvider GetServiceProvider()
        => Services;

    public override async ValueTask DisposeAsync()
    {
        // only dispose the clients of this instance
        // do not dispose this instance itself
        GuestClient.Dispose();

        // dispose the delegated factories of this instance
        foreach (var f in Factories)
            await f.DisposeAsync();
    }
}