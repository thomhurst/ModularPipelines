using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.SignalR.Hub;

namespace ModularPipelines.Distributed.SignalR.Server;

/// <summary>
/// Starts a lightweight ASP.NET Core server hosting the SignalR pipeline hub.
/// Runs on a background thread so the master pipeline can continue execution.
/// </summary>
internal sealed class MasterServerHost : IAsyncDisposable
{
    // Workers hold long-running invocations (claims, result and cancellation waits) alongside
    // heartbeats, so one connection must be able to run many invocations at once.
    private const int MaximumParallelInvocationsPerWorker = 1024;

    private WebApplication? _app;
    private CloudflaredTunnel? _tunnel;

    /// <summary>
    /// Gets the URL workers should connect to: the configured advertised URL, the tunnel URL, or
    /// the URL the server actually bound. Available after <see cref="StartAsync"/> completes.
    /// </summary>
    public Uri AdvertisedUrl { get; private set; } = null!;

    /// <summary>Gets the URL the server bound, including an OS-assigned port.</summary>
    public Uri BoundUrl { get; private set; } = null!;

    public async Task StartAsync(
        SignalRDistributedOptions options,
        SignalRMasterState masterState,
        string? accessToken,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseUrls(options.ListenUrl);
        builder.Logging.ClearProviders();
        builder.Logging.AddProvider(new ForwardingLoggerProvider(loggerFactory));

        builder.Services.AddSignalR(hubOptions =>
        {
            hubOptions.MaximumReceiveMessageSize = options.MaxMessageSizeBytes;
            hubOptions.MaximumParallelInvocationsPerClient = MaximumParallelInvocationsPerWorker;

            // Exception details can include paths and secrets; workers only need HubException messages.
            hubOptions.EnableDetailedErrors = false;

            // Detect a dead or silent worker quickly: KeepAliveInterval is how often the server pings
            // workers; ClientTimeoutInterval is how long without a message before a worker is gone.
            hubOptions.KeepAliveInterval = options.KeepAliveInterval;
            hubOptions.ClientTimeoutInterval = options.PeerTimeout;
        }).AddJsonProtocol(jsonOptions =>
        {
            // Match the client's default STJ options: PascalCase, case-insensitive
            jsonOptions.PayloadSerializerOptions.PropertyNamingPolicy = null;
            jsonOptions.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
        });
        builder.Services.AddSingleton(masterState);

        _app = builder.Build();
        if (accessToken is not null)
        {
            var expected = Encoding.UTF8.GetBytes(accessToken);
            _app.Use(async (context, next) =>
            {
                if (!IsAuthorized(context.Request, expected))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                await next(context).ConfigureAwait(false);
            });
        }

        _app.MapHub<DistributedPipelineHub>(options.HubPath);

        var logger = loggerFactory.CreateLogger<MasterServerHost>();

        // StartAsync completes only once Kestrel has bound to the port.
        await _app.StartAsync(cancellationToken).ConfigureAwait(false);

        // Use the actual bound URL (important when port 0 is used to get an OS-assigned port).
        BoundUrl = new Uri(_app.Urls.FirstOrDefault() ?? options.ListenUrl, UriKind.Absolute);
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation(
                "SignalR master listening at {Url}{Path} (access token {TokenState})",
                BoundUrl,
                options.HubPath,
                accessToken is null ? "not required" : "required");
        }

        AdvertisedUrl = options.AdvertisedUrl ?? BoundUrl;
        if (options.Tunnel.Enabled)
        {
            _tunnel = new CloudflaredTunnel();
            await _tunnel.StartAsync(BoundUrl, options.Tunnel, logger, cancellationToken).ConfigureAwait(false);
            AdvertisedUrl = options.AdvertisedUrl ?? _tunnel.PublicUrl!;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_tunnel is not null)
        {
            await _tunnel.DisposeAsync().ConfigureAwait(false);
        }

        if (_app is not null)
        {
            await _app.StopAsync().ConfigureAwait(false);
            await _app.DisposeAsync().ConfigureAwait(false);
        }
    }

    internal static bool IsAuthorized(HttpRequest request, byte[] expectedToken)
    {
        // Negotiation sends the token as a bearer header; WebSockets and server-sent events send it
        // in the access_token query string because browsers cannot set headers for them.
        var presented = request.Headers.Authorization.ToString() is { } header
                        && header.StartsWith("Bearer ", StringComparison.Ordinal)
            ? header["Bearer ".Length..]
            : request.Query["access_token"].ToString();
        return !string.IsNullOrEmpty(presented)
               && CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(presented), expectedToken);
    }

    /// <summary>
    /// Simple <see cref="ILoggerProvider"/> that forwards to an existing <see cref="ILoggerFactory"/>.
    /// </summary>
    private sealed class ForwardingLoggerProvider(ILoggerFactory factory) : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => factory.CreateLogger(categoryName);

        public void Dispose()
        {
        }
    }
}
