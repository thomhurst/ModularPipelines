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
    [Arguments("http://localhost:5099")]
    [Arguments("http://127.0.0.2:5099")]
    [Arguments("http://[::1]:5099")]
    [Arguments("https://0.0.0.0:5099")]
    public async Task Endpoints_Accept_Https_Or_Loopback_Http(string endpoint)
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddSignalRDistributedCoordinator(options => options with
        {
            ListenUrl = new Uri(endpoint),
            AdvertisedUrl = new Uri(endpoint),
        });
        using var services = builder.Services.BuildServiceProvider();

        var options = services.GetRequiredService<IOptions<SignalRDistributedOptions>>().Value;
        await Assert.That(options.ListenUrl).IsEqualTo(new Uri(endpoint));
        await Assert.That(options.AdvertisedUrl).IsEqualTo(new Uri(endpoint));
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Non_Loopback_Http_Endpoints_Fail_Validation(bool advertised, bool withToken)
    {
        foreach (var endpoint in new[] { "http://master.example:5099", "http://192.0.2.1:5099", "http://0.0.0.0:5099", "http://[::]:5099", "http://[2001:db8::1]:5099" })
        {
            var builder = Pipeline.CreateBuilder();
            builder.AddSignalRDistributedCoordinator(options => options with
            {
                ListenUrl = advertised ? options.ListenUrl : new Uri(endpoint),
                AdvertisedUrl = advertised ? new Uri(endpoint) : null,
                AccessToken = withToken ? "private-token" : null,
            });
            using var services = builder.Services.BuildServiceProvider();

            var exception = await Assert.That(() => services.GetRequiredService<IOptions<SignalRDistributedOptions>>().Value)
                .Throws<OptionsValidationException>()
                .WithMessageContaining(advertised ? nameof(SignalRDistributedOptions.AdvertisedUrl) : nameof(SignalRDistributedOptions.ListenUrl));

            await Assert.That(exception!.Message).Contains("HTTPS");
        }
    }

    [Test]
    [Arguments("relative/path")]
    [Arguments("ftp://master.example")]
    [Arguments("file:///master")]
    public async Task Advertised_Endpoint_Requires_An_Absolute_Http_Uri(string endpoint)
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddSignalRDistributedCoordinator(options => options with { AdvertisedUrl = new Uri(endpoint, UriKind.RelativeOrAbsolute) });
        using var services = builder.Services.BuildServiceProvider();

        await Assert.That(() => services.GetRequiredService<IOptions<SignalRDistributedOptions>>().Value)
            .Throws<OptionsValidationException>().WithMessageContaining(nameof(SignalRDistributedOptions.AdvertisedUrl));
    }

    [Test]
    [Arguments("http://private-user:private-password@master.example:5099/?secret=private-query", false, true)]
    [Arguments("http://private-user:private-password@master.example:5099/?secret=private-query", true, true)]
    [Arguments("http://master.example:5099", false, false)]
    [Arguments("http://master.example:5099", true, false)]
    [Arguments("http://192.0.2.1:5099", true, true)]
    [Arguments("http://0.0.0.0:5099", true, true)]
    [Arguments("http://[2001:db8::1]:5099", true, true)]
    [Arguments("relative/path", true, true)]
    [Arguments("ftp://master.example", true, true)]
    [Arguments((string?) null, true, true)]
    public async Task Worker_Rejects_Unsafe_Endpoints_Before_Connecting(string? endpoint, bool useDiscovery, bool withToken)
    {
        var url = endpoint is null ? null! : new Uri(endpoint, UriKind.RelativeOrAbsolute);
        var discovery = new SignalRTestMaster.CapturingDiscovery
        {
            Override = new MasterEndpoint { Url = url, AccessToken = withToken ? "private-discovered-token" : null },
        };
        var factory = new SignalRDistributedCoordinatorFactory(
            MsOptions.Create(new SignalRDistributedOptions
            {
                AdvertisedUrl = useDiscovery ? null : url,
                AccessToken = withToken ? "private-configured-token" : null,
            }),
            MsOptions.Create(new DistributedOptions()),
            NullLoggerFactory.Instance,
            useDiscovery ? discovery : null);

        var exception = await Assert.That(async () =>
                await factory.CreateWorkerAsync(CancellationToken.None).WaitAsync(TimeSpan.FromSeconds(5)))
            .Throws<InvalidOperationException>().WithMessageContaining("HTTPS");

        await Assert.That(exception!.Message).DoesNotContain("private");
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
