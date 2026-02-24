using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.SignalR.Configuration;

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
    public string? PublicUrl { get; private set; }

    public async Task StartAsync(
        string localUrl,
        SignalRDistributedOptions options,
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

        logger.LogInformation("Starting cloudflared tunnel for {Url}...", localUrl);

        _process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Failed to start cloudflared process.");

        // cloudflared writes the tunnel URL to stderr
        var urlTcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
        var timeout = TimeSpan.FromSeconds(options.TunnelStartupTimeoutSeconds);

        _process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data is null)
            {
                return;
            }

            logger.LogDebug("[cloudflared] {Line}", e.Data);

            // cloudflared outputs the tunnel URL in a box like:
            // |  https://random-words.trycloudflare.com  |
            // For named tunnels, the domain may be custom.
            var match = TunnelUrlRegex().Match(e.Data);
            if (match.Success && !match.Value.Contains("api.cloudflare.com", StringComparison.OrdinalIgnoreCase))
            {
                urlTcs.TrySetResult(match.Value);
            }
        };

        _process.OutputDataReceived += (_, e) =>
        {
            if (e.Data is not null)
            {
                logger.LogDebug("[cloudflared] {Line}", e.Data);
            }
        };

        _process.BeginErrorReadLine();
        _process.BeginOutputReadLine();

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(timeout);

        await using var reg = timeoutCts.Token.Register(() =>
            urlTcs.TrySetCanceled(timeoutCts.Token));

        try
        {
            PublicUrl = await urlTcs.Task;
            logger.LogInformation("Cloudflared tunnel established: {TunnelUrl}", PublicUrl);
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning(
                "Cloudflared tunnel URL was not detected within {Timeout}s. " +
                "If using a named tunnel with a custom domain, the URL regex may need updating.",
                options.TunnelStartupTimeoutSeconds);
            await DisposeAsync();
            throw new TimeoutException(
                $"Cloudflared did not provide a tunnel URL within {options.TunnelStartupTimeoutSeconds}s. " +
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
                await _process.WaitForExitAsync();
            }
            catch
            {
                // Best-effort cleanup
            }
        }

        _process?.Dispose();
    }

    [GeneratedRegex(@"https://[a-zA-Z0-9\-]+\.[a-zA-Z0-9\-\.]+")]
    private static partial Regex TunnelUrlRegex();
}
