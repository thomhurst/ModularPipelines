using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.SignalR.Configuration;
using ModularPipelines.Distributed.SignalR.Hub;

namespace ModularPipelines.Distributed.SignalR.Server;

/// <summary>
/// Starts a lightweight ASP.NET Core server hosting the SignalR pipeline hub.
/// Runs on a background thread so the master pipeline can continue execution.
/// </summary>
internal class MasterServerHost : IAsyncDisposable
{
    private WebApplication? _app;
    private CloudflaredTunnel? _tunnel;

    /// <summary>
    /// The real <see cref="IHubContext{THub}"/> from the WebApplication's DI container.
    /// Available after <see cref="StartAsync"/> completes.
    /// </summary>
    public IHubContext<DistributedPipelineHub> HubContext =>
        _app?.Services.GetRequiredService<IHubContext<DistributedPipelineHub>>()
        ?? throw new InvalidOperationException("Server not started.");

    /// <summary>
    /// The URL workers should connect to. If a tunnel is active, this is the public tunnel URL.
    /// Otherwise, it is the local MasterUrl.
    /// </summary>
    public string AdvertisedUrl { get; private set; } = string.Empty;

    public async Task StartAsync(
        SignalRDistributedOptions options,
        SignalRMasterState masterState,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseUrls(options.MasterUrl);
        builder.Logging.ClearProviders();
        builder.Logging.AddProvider(new ForwardingLoggerProvider(loggerFactory));

        builder.Services.AddSignalR(hubOptions =>
        {
            hubOptions.MaximumReceiveMessageSize = options.MaximumReceiveMessageSize;
            hubOptions.EnableDetailedErrors = true;
        }).AddJsonProtocol(jsonOptions =>
        {
            // Match the client's default STJ options: PascalCase, case-insensitive
            jsonOptions.PayloadSerializerOptions.PropertyNamingPolicy = null;
            jsonOptions.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
        });
        builder.Services.AddSingleton(masterState);

        _app = builder.Build();

        _app.MapHub<DistributedPipelineHub>(options.HubPath);

        var logger = loggerFactory.CreateLogger<MasterServerHost>();
        logger.LogInformation("Starting SignalR master server at {Url}{Path}", options.MasterUrl, options.HubPath);

        // StartAsync completes only once Kestrel has bound to the port — no race, no wasted time
        await _app.StartAsync(cancellationToken);

        // Use the actual bound URL (important when port 0 is used to get an OS-assigned port)
        AdvertisedUrl = _app.Urls.FirstOrDefault() ?? options.MasterUrl;

        if (options.EnableTunnel)
        {
            _tunnel = new CloudflaredTunnel();
            await _tunnel.StartAsync(options.MasterUrl, options, logger, cancellationToken);
            AdvertisedUrl = _tunnel.PublicUrl!;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_tunnel is not null)
        {
            await _tunnel.DisposeAsync();
        }

        if (_app is not null)
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }
    }

    /// <summary>
    /// Simple <see cref="ILoggerProvider"/> that forwards to an existing <see cref="ILoggerFactory"/>.
    /// </summary>
    private class ForwardingLoggerProvider(ILoggerFactory factory) : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => factory.CreateLogger(categoryName);
        public void Dispose() { }
    }
}
