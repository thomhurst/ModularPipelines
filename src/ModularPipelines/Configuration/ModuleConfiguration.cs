using ModularPipelines.Context;
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
/// and execution hooks.
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
    ///     .WithRetryCount(3)
    ///     .Build();
    /// </code>
    /// </example>
    public static ModuleConfigurationBuilder Create() => new();

    /// <summary>
    /// Gets the condition that determines whether the module should be skipped.
    /// </summary>
    /// <value>
    /// A function that takes an <see cref="IModuleContext"/> and returns a <see cref="Task{SkipDecision}"/>,
    /// or null if no skip condition is configured.
    /// </value>
    public Func<IModuleContext, Task<SkipDecision>>? SkipCondition { get; init; }

    /// <summary>
    /// Gets the timeout duration for module execution.
    /// </summary>
    /// <value>
    /// A <see cref="TimeSpan"/> representing the maximum execution time,
    /// or null if no timeout is configured.
    /// </value>
    public TimeSpan? Timeout { get; init; }

    /// <summary>
    /// Gets the factory function that creates a retry policy for module execution.
    /// </summary>
    /// <value>
    /// A function that takes an <see cref="IModuleContext"/> and returns an <see cref="IAsyncPolicy"/>,
    /// or null if no retry policy is configured.
    /// </value>
    public Func<IModuleContext, IAsyncPolicy>? RetryPolicyFactory { get; init; }

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
    /// Gets the hook to execute before the module's main execution.
    /// </summary>
    /// <value>
    /// A function that takes an <see cref="IModuleContext"/> and returns a <see cref="Task"/>,
    /// or null if no before-execution hook is configured.
    /// </value>
    public Func<IModuleContext, Task>? OnBeforeExecute { get; init; }

    /// <summary>
    /// Gets the hook to execute after the module's main execution.
    /// </summary>
    /// <value>
    /// A function that takes an <see cref="IModuleContext"/> and returns a <see cref="Task"/>,
    /// or null if no after-execution hook is configured.
    /// </value>
    public Func<IModuleContext, Task>? OnAfterExecute { get; init; }
}
