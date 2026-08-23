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
                    Random.Shared.NextDouble())));
    }

    internal static TimeSpan CalculateDelay(
        int retryAttempt,
        TimeSpan baseDelay,
        double jitterFactor)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(retryAttempt, 1);
        if (baseDelay < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(baseDelay), baseDelay, "The retry base delay cannot be negative.");
        }

        if (jitterFactor is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(jitterFactor));
        }

        if (baseDelay == TimeSpan.Zero)
        {
            return TimeSpan.Zero;
        }

        var exponentialTicks = baseDelay.Ticks * Math.Pow(2, retryAttempt - 1);
        var maximumTicks = Math.Min(exponentialTicks, TimeSpan.MaxValue.Ticks);
        var minimumTicks = maximumTicks / 2;
        var jitteredTicks = minimumTicks + ((maximumTicks - minimumTicks) * jitterFactor);

        if (jitteredTicks >= TimeSpan.MaxValue.Ticks)
        {
            return TimeSpan.MaxValue;
        }

        return TimeSpan.FromTicks((long) jitteredTicks);
    }
}
