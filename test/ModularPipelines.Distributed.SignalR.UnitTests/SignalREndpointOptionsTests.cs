using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class SignalREndpointOptionsTests
{
    [Test]
    [Arguments("relative/path")]
    [Arguments("ftp://master.example")]
    [Arguments("file:///master")]
    [Arguments((string?) null)]
    public async Task Listener_Requires_An_Absolute_Http_Uri(string? endpoint)
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddSignalRDistributedCoordinator(options =>
            options.ListenUrl = endpoint is null ? null! : new Uri(endpoint, UriKind.RelativeOrAbsolute));
        using var services = builder.Services.BuildServiceProvider();

        await Assert.That(() => services.GetRequiredService<IOptions<SignalRDistributedOptions>>().Value)
            .Throws<OptionsValidationException>().WithMessageContaining(nameof(SignalRDistributedOptions.ListenUrl));
    }

    [Test]
    [Arguments("http://127.0.0.1:0")]
    [Arguments("https://master.example:6000")]
    public async Task Listener_Accepts_Http_And_Https_Uris(string endpoint)
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddSignalRDistributedCoordinator(options => options.ListenUrl = new Uri(endpoint));
        using var services = builder.Services.BuildServiceProvider();

        await Assert.That(services.GetRequiredService<IOptions<SignalRDistributedOptions>>().Value.ListenUrl)
            .IsEqualTo(new Uri(endpoint));
    }

    [Test]
    public async Task Port_Zero_Listener_Advertises_Actual_Bound_Uri()
    {
        await using var master = await SignalRTestMaster.StartAsync();
        var endpoint = await master.Discovery.Endpoint;

        await Assert.That(endpoint.Url.Scheme).IsEqualTo("http");
        await Assert.That(endpoint.Url.IsLoopback).IsTrue();
        await Assert.That(endpoint.Url.Port).IsGreaterThan(0);
        await Assert.That(endpoint.AccessToken).IsNull();
        await master.ConnectWorkerAsync();
    }
}
