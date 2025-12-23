using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Options;

/// <summary>
/// Configuration options for controlling module execution concurrency.
/// </summary>
[ExcludeFromCodeCoverage]
public record ConcurrencyOptions
{
    /// <summary>
    /// Gets or sets the maximum number of modules that can execute in parallel.
    /// Default: <c>Environment.ProcessorCount * 4</c> for aggressive parallelism.
    /// Set to <see cref="int.MaxValue"/> for unlimited parallelism (bounded only by dependencies).
    /// </summary>
    /// <example>
    /// <code>
    /// // Limit to processor count (conservative)
    /// options.Concurrency.MaxParallelism = Environment.ProcessorCount;
    ///
    /// // Unlimited parallelism
    /// options.Concurrency.MaxParallelism = int.MaxValue;
    /// </code>
    /// </example>
    public int MaxParallelism { get; set; } = Environment.ProcessorCount * 4;

    /// <summary>
    /// Gets or sets the maximum number of CPU-intensive modules that can execute concurrently.
    /// Only applies to modules decorated with <c>[ExecutionHint(ExecutionType.CpuIntensive)]</c>.
    /// Default: <c>Environment.ProcessorCount</c>.
    /// Set to <c>null</c> to use <see cref="MaxParallelism"/> instead.
    /// </summary>
    public int? MaxCpuIntensiveModules { get; set; } = Environment.ProcessorCount;

    /// <summary>
    /// Gets or sets the maximum number of I/O-intensive modules that can execute concurrently.
    /// Only applies to modules decorated with <c>[ExecutionHint(ExecutionType.IoIntensive)]</c>.
    /// Default: <c>null</c> (unlimited, bounded only by <see cref="MaxParallelism"/>).
    /// </summary>
    public int? MaxIoIntensiveModules { get; set; }
}
