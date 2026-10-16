using Kevlar;

namespace ModularPipelines.Configuration;

internal static class ModuleRetryShieldFactory
{
    internal static Shield Create(ModuleRetryConfiguration configuration)
    {
        return Shield
            .When(configuration.ShouldRetry ?? (static _ => true))
            .Retry(
                configuration.Count,
                Backoff.Custom(retryAttempt => CalculateDelay(
                    retryAttempt,
                    configuration.BaseDelay,
                    configuration.MaxDelay,
                    Random.Shared.NextDouble())));
    }

    internal static TimeSpan CalculateDelay(
        int retryAttempt,
        TimeSpan baseDelay,
        TimeSpan maxDelay,
        double jitterFactor)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(retryAttempt, 1);
        if (baseDelay < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(baseDelay), baseDelay, "The retry base delay cannot be negative.");
        }

        if (maxDelay < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(maxDelay), maxDelay, "The retry maximum delay cannot be negative.");
        }

        if (jitterFactor is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(jitterFactor));
        }

        if (baseDelay == TimeSpan.Zero || maxDelay == TimeSpan.Zero)
        {
            return TimeSpan.Zero;
        }

        var exponentialTicks = baseDelay.Ticks * Math.Pow(2, retryAttempt - 1);
        var maximumTicks = Math.Min(exponentialTicks, maxDelay.Ticks);
        var minimumTicks = maximumTicks / 2;
        var jitteredTicks = minimumTicks + ((maximumTicks - minimumTicks) * jitterFactor);

        if (jitteredTicks >= maxDelay.Ticks)
        {
            return maxDelay;
        }

        return TimeSpan.FromTicks((long) jitteredTicks);
    }
}
