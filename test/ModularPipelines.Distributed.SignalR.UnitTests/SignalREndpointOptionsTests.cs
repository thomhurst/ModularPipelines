using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed.SignalR.Coordination;
using MsOptions = Microsoft.Extensions.Options.Options;

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
        builder.AddSignalRDistributedCoordinator(options => options with { ListenUrl = endpoint is null ? null! : new Uri(endpoint, UriKind.RelativeOrAbsolute) });
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
        builder.AddSignalRDistributedCoordinator(options => options with { ListenUrl = new Uri(endpoint) });
        using var services = builder.Services.BuildServiceProvider();

        await Assert.That(services.GetRequiredService<IOptions<SignalRDistributedOptions>>().Value.ListenUrl)
            .IsEqualTo(new Uri(endpoint));
    }

    [Test]
    [Arguments("https://proxy/ci/", "/pipeline-hub", "https://proxy/ci/pipeline-hub")]
    [Arguments("https://proxy/ci", "/pipeline-hub", "https://proxy/ci/pipeline-hub")]
    [Arguments("https://proxy", "/pipeline-hub", "https://proxy/pipeline-hub")]
    [Arguments("https://proxy/", "/pipeline-hub", "https://proxy/pipeline-hub")]
    [Arguments("https://proxy/ci///", "/pipeline-hub", "https://proxy/ci/pipeline-hub")]
    [Arguments("https://proxy/ci/", "/custom/pipeline-hub", "https://proxy/ci/custom/pipeline-hub")]
    [Arguments("https://proxy:8443/ci%20jobs/", "/pipeline-hub", "https://proxy:8443/ci%20jobs/pipeline-hub")]
    [Arguments("https://proxy/ci/?query=value#fragment", "/pipeline-hub", "https://proxy/ci/pipeline-hub")]
    public async Task Hub_Url_Appends_Hub_Path_To_Endpoint_Path(string endpointUrl, string hubPath, string expectedUrl)
    {
        var hubUrl = SignalRDistributedCoordinatorFactory.GetHubUrl(new Uri(endpointUrl), hubPath);

        await Assert.That(hubUrl.AbsoluteUri).IsEqualTo(expectedUrl);
    }

    [Test]
    [Arguments("", false)]
    [Arguments("", true)]
    [Arguments("ci", false)]
    [Arguments("ci", true)]
    [Arguments("ci/", false)]
    [Arguments("ci/", true)]
    public async Task Worker_Connects_Using_Endpoint_Path_Prefix(string pathPrefix, bool useDiscovery)
    {
        var serverHubPath = pathPrefix.Length == 0 ? "/pipeline-hub" : "/ci/pipeline-hub";
        await using var master = await SignalRTestMaster.StartAsync(options => options with { HubPath = serverHubPath });
        var endpoint = await master.Discovery.Endpoint;
        var workerUrl = new Uri(endpoint.Url, pathPrefix);
        var options = new SignalRDistributedOptions
        {
            AdvertisedUrl = useDiscovery ? null : workerUrl,
            ConnectionTimeout = TimeSpan.FromSeconds(10),
            MaxReconnectAttempts = 0,
        };
        master.Discovery.Override = endpoint with { Url = workerUrl };
        var factory = new SignalRDistributedCoordinatorFactory(
            MsOptions.Create(options),
            MsOptions.Create(new DistributedOptions { RunId = SignalRTestMaster.RunId }),
            NullLoggerFactory.Instance,
            useDiscovery ? master.Discovery : null);

        await using var worker = (SignalRWorkerCoordinator) await factory.CreateWorkerAsync(CancellationToken.None);
        await worker.RegisterWorkerAsync(SignalRTestMaster.Registration("path-prefix-worker"), CancellationToken.None);

        var workers = await master.Master.GetRegisteredWorkersAsync(CancellationToken.None);
        await Assert.That(workers.Select(x => x.WorkerId)).Contains(new WorkerId("path-prefix-worker"));
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
