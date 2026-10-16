using ModularPipelines.Configuration;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal sealed record ParallelLimitConfiguration(object Key, int Limit)
{
    public static ParallelLimitConfiguration? Resolve(ModuleConfiguration configuration, ConcurrencyOptions options)
    {
        if (configuration.ParallelLimitKey is { } key)
        {
            return new ParallelLimitConfiguration(key, options.GetLimit(key));
        }

        if (configuration.ParallelLimiter is not { } limiter)
        {
            return null;
        }

        var limit = limiter.Limit;
        if (limit <= 0)
        {
            throw new ArgumentException(
                $"Parallel limit for type '{limiter.Type.FullName}' must be a positive integer, but was {limit}.");
        }

        return new ParallelLimitConfiguration(limiter.Type, limit);
    }
}
