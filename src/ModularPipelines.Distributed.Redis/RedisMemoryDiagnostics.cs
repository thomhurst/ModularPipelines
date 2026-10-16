using System.Globalization;
using Microsoft.Extensions.Logging;
using Respire;

namespace ModularPipelines.Distributed.Redis;

/// <summary>
/// Best-effort diagnostics for the Redis server answering INFO. These observations do not
/// certify other cluster nodes or prove why an individual artifact key disappeared.
/// </summary>
internal static class RedisMemoryDiagnostics
{
    internal sealed record EvictionSnapshot(string RunId, long EvictedKeys);

    internal static async Task CheckMemoryPolicyAsync(
        IRespireClient redis,
        RedisOptions options,
        ILogger? logger,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        string warning;
        Exception? diagnosticFailure = null;
        try
        {
            var info = ParseInfo(await redis.Server.InfoAsync("memory", cancellationToken).ConfigureAwait(false));
            if (!TryReadCounter(info, "maxmemory", out var maxMemory)
                || (maxMemory > 0 && (!info.TryGetValue("maxmemory_policy", out var policy) || string.IsNullOrWhiteSpace(policy))))
            {
                warning = "Redis INFO memory did not report a usable maxmemory configuration; eviction safety is unknown.";
            }
            else if (maxMemory == 0 || string.Equals(info["maxmemory_policy"], "noeviction", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            else
            {
                warning = $"Redis has finite maxmemory ({maxMemory} bytes) with policy '{info["maxmemory_policy"]}'. "
                    + "Run state and artifact chunks can be evicted before their TTL expires. Use noeviction or unlimited maxmemory.";
            }
        }
        catch (Exception exception) when (IsDiagnosticFailure(exception))
        {
            diagnosticFailure = exception;
            warning = "Redis INFO memory is unavailable; eviction safety is unknown. Allow INFO diagnostics or verify maxmemory and maxmemory_policy independently.";
        }

        if (options.FailOnUnsafeMemoryPolicy)
        {
            throw new InvalidOperationException(warning, diagnosticFailure);
        }

        logger?.LogWarning(diagnosticFailure, "{RedisMemoryWarning}", warning);
    }

    internal static async Task<EvictionSnapshot?> ReadEvictionSnapshotAsync(IRespireClient redis, CancellationToken cancellationToken)
    {
        try
        {
            // One response keeps the process identity and its counter together. Default INFO includes
            // both server and stats. A new process/node must never be compared to the old counter.
            var info = ParseInfo(await redis.Server.InfoAsync(null, cancellationToken).ConfigureAwait(false));
            if (info.TryGetValue("run_id", out var runId) && !string.IsNullOrWhiteSpace(runId)
                && TryReadCounter(info, "evicted_keys", out var evictedKeys))
            {
                return new EvictionSnapshot(runId, evictedKeys);
            }
        }
        catch (Exception exception) when (IsDiagnosticFailure(exception))
        {
            // Diagnostics must not replace an artifact integrity failure.
        }

        return null;
    }

    internal static async Task<string> DescribeEvictionsAsync(
        IRespireClient redis,
        EvictionSnapshot? before,
        CancellationToken cancellationToken)
    {
        EvictionSnapshot? after;
        try
        {
            after = before is null ? null : await ReadEvictionSnapshotAsync(redis, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // The integrity failure is already known; retain it if cancellation races with this diagnostic.
            after = null;
        }

        if (before is null || after is null || before.RunId != after.RunId || after.EvictedKeys < before.EvictedKeys)
        {
            return " Redis evicted_keys delta unavailable (INFO denied, missing, interrupted, or server/counter changed).";
        }

        return $" Observed Redis server evicted_keys delta during download: {after.EvictedKeys - before.EvictedKeys}. "
            + "This server-wide counter does not prove that this artifact was evicted.";
    }

    private static bool IsDiagnosticFailure(Exception exception) =>
        exception is RespireException or IOException or TimeoutException or NotSupportedException;

    private static bool TryReadCounter(Dictionary<string, string> info, string name, out long value)
    {
        value = 0;
        return info.TryGetValue(name, out var text)
            && long.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out value)
            && value >= 0;
    }

    private static Dictionary<string, string> ParseInfo(string text)
    {
        var values = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var line in text.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
        {
            var separator = line.IndexOf(':');
            if (separator > 0 && line[0] != '#')
            {
                values[line[..separator]] = line[(separator + 1)..];
            }
        }

        return values;
    }
}
