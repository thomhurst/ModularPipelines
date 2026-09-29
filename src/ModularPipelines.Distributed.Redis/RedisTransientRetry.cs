using Kevlar;
using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis;

/// <summary>
/// Builds the Kevlar shield that retries idempotent Redis commands after a timeout or a dropped
/// connection. A large value is one command on a multiplexed connection, so its transfer can time
/// out while other processes saturate the link even though the server is healthy.
/// </summary>
internal static class RedisTransientRetry
{
    /// <summary>The number of times a command is sent before its failure is surfaced.</summary>
    internal const int MaxAttempts = 4;

    /// <summary>
    /// The nominal delay before the first retry; each later retry doubles it. Equal jitter scales
    /// each delay by a random factor in [0.5, 1.5) so runners that failed together do not retry
    /// together.
    /// </summary>
    internal static readonly TimeSpan DefaultBaseDelay = TimeSpan.FromMilliseconds(500);

    /// <summary>
    /// Creates a shield that retries only transient Redis failures. Build it once and reuse it;
    /// cancellation stops both the command and any pending backoff.
    /// </summary>
    internal static Shield Create(TimeSpan baseDelay) =>
        Shield
            .When(IsTransient)
            .Retry(MaxAttempts - 1, Backoff.Exponential(baseDelay, 2, null, Jitter.Equal));

    internal static bool IsTransient(Exception exception) =>
        exception is RedisTimeoutException or RedisConnectionException;
}
