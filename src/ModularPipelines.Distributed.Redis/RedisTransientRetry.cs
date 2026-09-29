using StackExchange.Redis;

namespace ModularPipelines.Distributed.Redis;

/// <summary>
/// Retries idempotent Redis commands that failed with a timeout or a dropped connection.
/// A large value is one command on a multiplexed connection, so its transfer can time out while
/// other processes saturate the link even though the server is healthy.
/// </summary>
internal static class RedisTransientRetry
{
    /// <summary>The number of times a command is sent before its failure is surfaced.</summary>
    internal const int MaxAttempts = 4;

    /// <summary>
    /// The nominal delay before the first retry; each later retry doubles it. Each delay is jittered
    /// between half and one and a half times its nominal value so runners that failed together do
    /// not retry together.
    /// </summary>
    internal static readonly TimeSpan DefaultBaseDelay = TimeSpan.FromMilliseconds(500);

    public static async Task<T> ExecuteAsync<T>(
        Func<Task<T>> operation,
        TimeSpan baseDelay,
        CancellationToken cancellationToken)
    {
        for (var attempt = 1; ; attempt++)
        {
            try
            {
                return await operation().ConfigureAwait(false);
            }
            catch (Exception exception) when (
                attempt < MaxAttempts
                && IsTransient(exception)
                && !cancellationToken.IsCancellationRequested)
            {
                var delay = baseDelay * (1 << (attempt - 1)) * (0.5 + Random.Shared.NextDouble());
                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    internal static bool IsTransient(Exception exception) =>
        exception is RedisTimeoutException or RedisConnectionException;
}
