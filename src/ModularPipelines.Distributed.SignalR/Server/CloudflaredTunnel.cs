using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Distributed.SignalR.Server;

/// <summary>
/// Starts a cloudflared quick tunnel to expose a local port publicly.
/// Workers on remote machines connect via the tunnel URL.
/// </summary>
internal sealed partial class CloudflaredTunnel : IAsyncDisposable
{
    private Process? _process;

    /// <summary>
    /// The public HTTPS URL provided by cloudflared.
    /// Available after <see cref="StartAsync"/> completes.
    /// </summary>
    public Uri? PublicUrl { get; private set; }

    public async Task StartAsync(
        Uri localUrl,
        SignalRTunnelOptions options,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = options.CloudflaredPath,
            Arguments = $"tunnel --url {localUrl} --no-autoupdate",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        logger.LogInformation("Starting cloudflared tunnel for the SignalR master");

        _process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Failed to start cloudflared process.");

        // cloudflared writes the tunnel URL to stderr
        var urlTcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
        _process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data is null)
            {
                return;
            }

            // cloudflared output contains the public tunnel URL, so it is only logged at Trace.
            logger.LogTrace("Cloudflared: {Line}", e.Data);

            // cloudflared quick tunnels output the URL in a box like:
            // |  https://random-words.trycloudflare.com  |
            var match = TunnelUrlRegex().Match(e.Data);
            if (match.Success)
            {
                urlTcs.TrySetResult(match.Value);
            }
        };

        _process.OutputDataReceived += (_, e) =>
        {
            if (e.Data is not null)
            {
                logger.LogTrace("Cloudflared: {Line}", e.Data);
            }
        };

        _process.BeginErrorReadLine();
        _process.BeginOutputReadLine();

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(options.StartupTimeout);

        var registration = timeoutCts.Token.Register(() =>
            urlTcs.TrySetCanceled(timeoutCts.Token));
        await using var registrationScope = registration.ConfigureAwait(false);

        try
        {
            PublicUrl = new Uri(await urlTcs.Task.ConfigureAwait(false), UriKind.Absolute);

            // The public URL admits anyone who knows it, so keep it out of ordinary logs.
            logger.LogInformation("Cloudflared tunnel established");
            logger.LogDebug("Cloudflared tunnel URL: {TunnelUrl}", PublicUrl);
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning(
                "Cloudflared tunnel URL was not detected within {Timeout}. " +
                "If using a named tunnel with a custom domain, the URL regex may need updating.",
                options.StartupTimeout);
            await DisposeAsync().ConfigureAwait(false);
            throw new TimeoutException(
                $"Cloudflared did not provide a tunnel URL within {options.StartupTimeout}. " +
                "Ensure 'cloudflared' is installed and accessible on PATH.");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_process is { HasExited: false })
        {
            try
            {
                _process.Kill(entireProcessTree: true);
                await _process.WaitForExitAsync().ConfigureAwait(false);
            }
            catch
            {
                // Best-effort cleanup
            }
        }

        _process?.Dispose();
    }

    [GeneratedRegex(@"https://[a-zA-Z0-9\-]+\.trycloudflare\.com")]
    private static partial Regex TunnelUrlRegex();
}
