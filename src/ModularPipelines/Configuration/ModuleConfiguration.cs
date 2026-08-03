using System.Collections.Frozen;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using Polly;

namespace ModularPipelines.Configuration;

/// <summary>
/// Represents the configuration for a module's execution behavior.
/// </summary>
/// <remarks>
/// <para>
/// This class provides a unified, immutable configuration object that controls various aspects
/// of module execution including skip conditions, timeouts, retry policies, failure handling,
/// and scheduling metadata.
/// </para>
/// <para>
/// Use <see cref="Create"/> to obtain a <see cref="ModuleConfigurationBuilder"/> for fluent configuration,
/// or use <see cref="Default"/> for a configuration with all default values.
/// </para>
/// </remarks>
public sealed class ModuleConfiguration
{
    /// <summary>
    /// Gets the default module configuration with all properties set to their default values.
    /// </summary>
    /// <value>
    /// A <see cref="ModuleConfiguration"/> instance where all nullable properties are null
    /// and <see cref="AlwaysRun"/> is false.
    /// </value>
    public static ModuleConfiguration Default { get; } = new();

    /// <summary>
    /// Creates a new <see cref="ModuleConfigurationBuilder"/> for fluent configuration.
    /// </summary>
    /// <returns>A new <see cref="ModuleConfigurationBuilder"/> instance.</returns>
    /// <example>
    /// <code>
    /// var config = ModuleConfiguration.Create()
    ///     .WithTimeout(TimeSpan.FromMinutes(5))
    ///     .WithRetry(3)
    ///     .Build();
    /// </code>
    /// </example>
    public static ModuleConfigurationBuilder Create() => new();

    /// <summary>
    /// Gets the condition that determines whether the module should be skipped.
    /// </summary>
    /// <value>
    /// A function that takes an <see cref="IModuleContext"/> and <see cref="CancellationToken"/>
    /// and returns a <see cref="ValueTask{SkipDecision}"/>,
    /// or null if no skip condition is configured.
    /// </value>
    public Func<IModuleContext, CancellationToken, ValueTask<SkipDecision>>? SkipCondition { get; init; }

    internal Func<IModuleContext, CancellationToken, ValueTask<SkipDecision?>>? PlanningSkipCondition { get; init; }

    internal Func<IModuleContext, CancellationToken, ValueTask<SkipDecision?>>? SynchronousPlanningSkipCondition { get; init; }

    /// <summary>
    /// Gets the timeout duration for module execution.
    /// </summary>
    /// <value>
    /// A <see cref="TimeSpan"/> representing the maximum execution time,
    /// or null if no timeout is configured.
    /// </value>
    public TimeSpan? Timeout { get; init; }

    /// <summary>
    /// Gets the condition that determines whether a failure should be ignored.
    /// </summary>
    /// <value>
    /// A function that takes an <see cref="IModuleContext"/> and an <see cref="Exception"/>,
    /// returning a <see cref="Task{Boolean}"/> indicating whether to ignore the failure,
    /// or null if failures should not be ignored.
    /// </value>
    public Func<IModuleContext, Exception, Task<bool>>? IgnoreFailuresCondition { get; init; }

    /// <summary>
    /// Gets a value indicating whether this module should always run,
    /// even if other modules have failed.
    /// </summary>
    /// <value>
    /// true if the module should always run; otherwise, false.
    /// </value>
    public bool AlwaysRun { get; init; }

    /// <summary>
    /// Gets the keys that prevent this module from running in parallel with modules using the same keys.
    /// An empty collection prevents all parallel execution; <see langword="null"/> allows parallel execution.
    /// </summary>
    public IReadOnlyList<string>? ParallelConstraintKeys { get; init; }

    /// <summary>
    /// Gets the scheduling priority, or <see langword="null"/> to use normal priority.
    /// </summary>
    public ModulePriority? Priority { get; init; }

    /// <summary>
    /// Gets the resource-usage hint, or <see langword="null"/> to use the default execution type.
    /// </summary>
    public ExecutionType? ExecutionType { get; init; }

    /// <summary>
    /// Gets module tags used by metadata-based dependency selection.
    /// </summary>
    public IReadOnlySet<string> Tags { get; init; } = FrozenSet<string>.Empty;

    /// <summary>
    /// Gets the module category used by filtering and metadata-based dependency selection.
    /// </summary>
    public string? Category { get; init; }

    /// <summary>
    /// Gets file paths and glob patterns included in the module cache fingerprint.
    /// </summary>
    public IReadOnlyList<string> CacheInputPatterns { get; init; } = [];

    /// <summary>
    /// Gets explicit values included in the module cache fingerprint.
    /// </summary>
    public IReadOnlyList<string> CacheKeyParts { get; init; } = [];

    /// <summary>
    /// Gets environment variable names whose current values are included in the module cache fingerprint.
    /// </summary>
    public IReadOnlyList<string> CacheEnvironmentVariables { get; init; } = [];

    /// <summary>
    /// Gets a value indicating whether fingerprint-based caching is enabled for this module.
    /// </summary>
    /// <remarks>
    /// The fingerprint includes direct dependency results. A dependency's result must change whenever
    /// upstream state relevant to a cached dependent changes; transitive dependencies are not fingerprinted directly.
    /// </remarks>
    public bool CacheEnabled =>
        CacheInputPatterns.Count > 0
        || CacheKeyParts.Count > 0
        || CacheEnvironmentVariables.Count > 0;

    /// <summary>
    /// Gets dependencies declared through fluent configuration or attributes.
    /// </summary>
    internal IReadOnlyList<DeclaredDependency> Dependencies { get; init; } = [];

    /// <summary>
    /// Gets the standard retry configuration for module execution.
    /// </summary>
    internal ModuleRetryConfiguration? RetryConfiguration { get; init; }

    /// <summary>
    /// Gets the advanced policy factory for module execution.
    /// </summary>
    internal Func<IModuleContext, IAsyncPolicy>? AdvancedRetryPolicyFactory { get; init; }
}
