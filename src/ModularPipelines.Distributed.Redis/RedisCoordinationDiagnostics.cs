using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Respire;

namespace ModularPipelines.Distributed.Redis;

/// <summary>
/// Samples a separate connection after a coordination failure. A successful probe narrows the
/// investigation to the affected connection/path; it does not prove the original command ran.
/// </summary>
internal sealed class RedisCoordinationDiagnostics : IAsyncDisposable
{
    internal static readonly TimeSpan ProbeTimeout = TimeSpan.FromSeconds(3);
    internal static readonly TimeSpan ProbeInterval = TimeSpan.FromMinutes(1);

    private readonly Lock _gate = new();
    private readonly Func<CancellationToken, Task<IRespireClient>> _connect;
    private readonly ILogger? _logger;
    private readonly TimeProvider _clock;
    private Task _probe = Task.CompletedTask;
    private long? _lastProbe;
    private bool _disposed;

    public RedisCoordinationDiagnostics(RedisOptions options, ILogger? logger)
        : this(async token =>
        {
            var configuration = RedisConnectionProvider.CreateConfiguration(options) with
            {
                ConnectTimeout = ProbeTimeout,
                Connections = 1,
                CommandTimeout = ProbeTimeout,
                RetryPolicy = null,
                CommandHook = null,
                ConnectionHook = null,
                LoggerFactory = null,
            };
#pragma warning disable RESP010 // Deliberately independent, short-lived failure probe; never used for pipeline work.
            return await RespireClient.ConnectAsync(configuration, token).ConfigureAwait(false);
#pragma warning restore RESP010
        }, logger)
    {
    }

    internal RedisCoordinationDiagnostics(
        Func<CancellationToken, Task<IRespireClient>> connect,
        ILogger? logger,
        TimeProvider? clock = null)
    {
        _connect = connect;
        _logger = logger;
        _clock = clock ?? TimeProvider.System;
    }

    // Never await a probe on the heartbeat path: its original failure must reach the watchdog
    // promptly. Coalesce concurrent liveness/heartbeat failures and bound diagnostic traffic.
    internal Task ReportFailure(string operation)
    {
        if (_logger?.IsEnabled(LogLevel.Warning) != true)
        {
            return Task.CompletedTask;
        }

        lock (_gate)
        {
            if (_disposed || !_probe.IsCompleted
                || (_lastProbe is { } last && _clock.GetElapsedTime(last) < ProbeInterval))
            {
                return _probe;
            }

            _lastProbe = _clock.GetTimestamp();
            _probe = Task.Run(() => ProbeAsync(operation));
            return _probe;
        }
    }

    public async ValueTask DisposeAsync()
    {
        Task probe;
        lock (_gate)
        {
            _disposed = true;
            probe = _probe;
        }

        await probe.ConfigureAwait(false);
    }

    private async Task ProbeAsync(string operation)
    {
        using var timeout = new CancellationTokenSource(ProbeTimeout);
        var started = Stopwatch.GetTimestamp();
        try
        {
            var connection = await _connect(timeout.Token).ConfigureAwait(false);
            await using var ownedConnection = connection.ConfigureAwait(false);
            await connection.PingAsync(timeout.Token).ConfigureAwait(false);
            _logger?.LogWarning(
                "Redis {Operation} failed, but an independent connection and PING succeeded in {ElapsedMilliseconds} ms. "
                + "The pipeline process is alive; compare the original timeout stage and other runners before attributing the stall to the server or network. Lease deadlines remain enforced.",
                operation, Stopwatch.GetElapsedTime(started).TotalMilliseconds);

            // Each optional command can be denied independently by a managed service. Never log
            // SLOWLOG arguments/client identities or arbitrary INFO fields: they may contain secrets.
            await ReadServerDiagnosticsAsync(connection, timeout.Token).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            // Diagnostics must never replace a coordination error or expose endpoints/credentials
            // from exception messages. Cancellation is also a best-effort diagnostic outcome.
            _logger?.LogWarning(
                "Redis {Operation} independent probe did not complete ({FailureType}) after {ElapsedMilliseconds} ms. "
                + "The pipeline process is alive; server reachability is unknown. Probe budget: {BudgetMilliseconds} ms. Lease deadlines remain enforced.",
                operation, exception.GetType().Name, Stopwatch.GetElapsedTime(started).TotalMilliseconds, ProbeTimeout.TotalMilliseconds);
        }
    }

    private async Task ReadServerDiagnosticsAsync(IRespireClient connection, CancellationToken token)
    {
        try
        {
            var entries = await connection.Server.SlowLogAsync(5, token).ConfigureAwait(false);
            _logger?.LogWarning("Redis SLOWLOG returned {Count} recent entries (server-wide, not proof of this command's latency)", entries.Length);
            foreach (var entry in entries)
            {
                _logger?.LogWarning("Redis slow command at {Timestamp}: {DurationMilliseconds} ms", entry.Timestamp, entry.Duration.TotalMilliseconds);
            }
        }
        catch (Exception exception)
        {
            _logger?.LogWarning("Redis SLOWLOG diagnostics unavailable ({FailureType})", exception.GetType().Name);
        }

        try
        {
            var info = await connection.Server.InfoAsync("stats", token).ConfigureAwait(false);
            foreach (var line in info.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            {
                var separator = line.IndexOf(':');
                if (separator > 0
                    && line[..separator] is "latest_fork_usec" or "rejected_connections" or "total_error_replies" or "evicted_keys"
                    && long.TryParse(line.AsSpan(separator + 1), out var value) && value >= 0)
                {
                    _logger?.LogWarning("Redis server-wide {Counter}: {Value}", line[..separator], value);
                }
            }
        }
        catch (Exception exception)
        {
            _logger?.LogWarning("Redis INFO stats diagnostics unavailable ({FailureType})", exception.GetType().Name);
        }
    }
}
