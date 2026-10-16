using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Constants;

namespace ModularPipelines.Options;

/// <summary>
/// Configuration options for controlling module execution concurrency.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record ConcurrencyOptions
{
    private readonly FrozenDictionary<string, int> _limits = [];

    /// <summary>
    /// Gets named parallel limits selected by <see cref="ModuleConfigurationBuilder.WithParallelLimit"/>.
    /// Keys are case-sensitive and capacities must be positive. Configure before building the pipeline;
    /// changing capacities while a pipeline is running is not supported.
    /// Named limits are independent of typed limiters and <see cref="NotInParallelAttribute"/> constraint keys.
    /// </summary>
    /// <example>
    /// <code>
    /// builder.ConfigureConcurrency(options => options with
    /// {
    ///     Limits = new Dictionary&lt;string, int&gt; { ["docker"] = isCI ? 2 : 8 },
    /// });
    /// </code>
    /// </example>
    public IReadOnlyDictionary<string, int> Limits
    {
        get => _limits;
        init
        {
            ArgumentNullException.ThrowIfNull(value);
            _limits = value.ToFrozenDictionary(StringComparer.Ordinal);
        }
    }

    internal int GetLimit(string key)
    {
        if (!Limits.TryGetValue(key, out var limit))
        {
            throw new ArgumentException($"No parallel limit is configured for key '{key}'. Configure Concurrency.Limits before building the pipeline.");
        }

        if (limit <= 0)
        {
            throw new ArgumentException($"Parallel limit for key '{key}' must be a positive integer, but was {limit}.");
        }

        return limit;
    }

    /// <summary>
    /// Gets the maximum number of modules that can execute in parallel.
    /// Default: <c>Environment.ProcessorCount * 4</c> for aggressive parallelism.
    /// Set to <see cref="int.MaxValue"/> for unlimited parallelism (bounded only by dependencies).
    /// </summary>
    /// <example>
    /// <code>
    /// // Limit to processor count (conservative)
    /// builder.ConfigureConcurrency(concurrency => concurrency with
    /// {
    ///     MaxParallelism = Environment.ProcessorCount,
    /// });
    ///
    /// // Unlimited parallelism
    /// builder.ConfigureConcurrency(concurrency => concurrency with { MaxParallelism = int.MaxValue });
    /// </code>
    /// </example>
    public int MaxParallelism { get; init; } = Environment.ProcessorCount * ConcurrencyConstants.ParallelismMultiplier;

    /// <summary>
    /// Gets the maximum number of CPU-bound modules that can execute concurrently.
    /// Only applies to modules decorated with <c>[ExecutionHint(ExecutionHint.CpuBound)]</c>.
    /// Default: <c>Environment.ProcessorCount</c>.
    /// Set to <c>null</c> to use <see cref="MaxParallelism"/> instead.
    /// </summary>
    public int? MaxCpuIntensiveModules { get; init; } = Environment.ProcessorCount;

    /// <summary>
    /// Gets the maximum number of I/O-bound modules that can execute concurrently.
    /// Only applies to modules decorated with <c>[ExecutionHint(ExecutionHint.IoBound)]</c>.
    /// Default: <c>null</c> (unlimited, bounded only by <see cref="MaxParallelism"/>).
    /// </summary>
    public int? MaxIoIntensiveModules { get; init; }

    /// <summary>
    /// Gets the confirmation delay before treating a blocked scheduler as deadlocked.
    /// </summary>
    public TimeSpan NotificationTimeout { get; init; } = TimeSpan.FromMilliseconds(100);
}
