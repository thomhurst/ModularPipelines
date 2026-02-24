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

    /// <summary>
    /// The real <see cref="IHubContext{THub}"/> from the WebApplication's DI container.
    /// Available after <see cref="StartAsync"/> completes.
    /// </summary>
    public IHubContext<DistributedPipelineHub> HubContext =>
        _app?.Services.GetRequiredService<IHubContext<DistributedPipelineHub>>()
        ?? throw new InvalidOperationException("Server not started.");

    public async Task StartAsync(
        SignalRDistributedOptions options,
        SignalRMasterState masterState,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var builder = WebApplication.CreateSlimBuilder();
        builder.WebHost.UseUrls(options.MasterUrl);
        builder.Logging.ClearProviders();
        builder.Logging.AddProvider(new ForwardingLoggerProvider(loggerFactory));

        builder.Services.AddSignalR(hubOptions =>
        {
            hubOptions.MaximumReceiveMessageSize = options.MaximumReceiveMessageSize;
            hubOptions.EnableDetailedErrors = true;
        });
        builder.Services.AddSingleton(masterState);

        _app = builder.Build();

        _app.MapHub<DistributedPipelineHub>(options.HubPath, hubOptions =>
        {
            // Configure hub filter to inject master state
        });

        var logger = loggerFactory.CreateLogger<MasterServerHost>();
        logger.LogInformation("Starting SignalR master server at {Url}{Path}", options.MasterUrl, options.HubPath);

        // StartAsync completes only once Kestrel has bound to the port — no race, no wasted time
        await _app.StartAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
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
