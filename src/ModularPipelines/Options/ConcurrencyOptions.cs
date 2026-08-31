using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Constants;

namespace ModularPipelines.Options;

/// <summary>
/// Configuration options for controlling module execution concurrency.
/// </summary>
[ExcludeFromCodeCoverage]
public record ConcurrencyOptions
{
    /// <summary>
    /// Gets the maximum number of modules that can execute in parallel.
    /// Default: <c>Environment.ProcessorCount * 4</c> for aggressive parallelism.
    /// Set to <see cref="int.MaxValue"/> for unlimited parallelism (bounded only by dependencies).
    /// </summary>
    /// <example>
    /// <code>
    /// // Limit to processor count (conservative)
    /// options with
    /// {
    ///     Concurrency = options.Concurrency with { MaxParallelism = Environment.ProcessorCount },
    /// };
    ///
    /// // Unlimited parallelism
    /// options with
    /// {
    ///     Concurrency = options.Concurrency with { MaxParallelism = int.MaxValue },
    /// };
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
