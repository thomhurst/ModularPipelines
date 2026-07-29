using Polly;

namespace ModularPipelines.Configuration;

internal static class ModuleRetryPolicyFactory
{
    internal static IAsyncPolicy Create(ModuleRetryConfiguration configuration)
    {
        var policyBuilder = configuration.ShouldRetry is null
            ? Policy.Handle<Exception>()
            : Policy.Handle<Exception>(configuration.ShouldRetry);

        return policyBuilder.WaitAndRetryAsync(
            configuration.Count,
            retryAttempt => CalculateDelay(
                retryAttempt,
                configuration.BaseDelay,
                Random.Shared.NextDouble()));
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

        var exponentialTicks = baseDelay.Ticks * Math.Pow(2, retryAttempt - 1);
        var maximumTicks = Math.Min(exponentialTicks, TimeSpan.MaxValue.Ticks);
        var minimumTicks = maximumTicks / 2;
        var jitteredTicks = minimumTicks + ((maximumTicks - minimumTicks) * jitterFactor);

        return TimeSpan.FromTicks((long) jitteredTicks);
    }
}
