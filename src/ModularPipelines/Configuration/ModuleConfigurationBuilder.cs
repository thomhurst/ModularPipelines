using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Polly;

namespace ModularPipelines.Configuration;

/// <summary>
/// A fluent builder for creating <see cref="ModuleConfiguration"/> instances.
/// </summary>
/// <remarks>
/// <para>
/// This builder provides a fluent API for configuring module behavior including
/// skip conditions, timeouts, retry policies, failure handling, and scheduling metadata.
/// </para>
/// <para>
/// All methods return the builder instance to support method chaining.
/// Call <see cref="Build"/> to create the final <see cref="ModuleConfiguration"/> instance.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var config = ModuleConfiguration.Create()
///     .WithSkipWhen(_ => someCondition
///         ? SkipDecision.Skip("Configured skip condition returned true")
///         : SkipDecision.DoNotSkip)
///     .WithTimeout(TimeSpan.FromMinutes(5))
///     .WithRetryCount(3)
///     .WithIgnoreFailures()
///     .WithAlwaysRun()
///     .Build();
/// </code>
/// </example>
public sealed class ModuleConfigurationBuilder
{
    private readonly HashSet<string> _tags = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<DeclaredDependency> _dependencies = [];
    private readonly List<Func<IModuleContext, CancellationToken, ValueTask<SkipDecision>>> _skipConditions = [];
    private TimeSpan? _timeout;
    private Func<IModuleContext, IAsyncPolicy>? _retryPolicyFactory;
    private Func<IModuleContext, Exception, Task<bool>>? _ignoreFailuresCondition;
    private bool _alwaysRun;
    private string[]? _parallelConstraintKeys;
    private ModulePriority? _priority;
    private ExecutionType? _executionType;
    private string? _category;

    #region WithSkipWhen Overloads

    /// <summary>
    /// Adds a synchronous skip condition.
    /// </summary>
    /// <param name="condition">A function that receives the module context and returns a <see cref="SkipDecision"/>.</param>
    /// <returns>This builder instance for method chaining.</returns>
    /// <remarks>Repeated conditions are combined with AND semantics and evaluated in registration order.</remarks>
    public ModuleConfigurationBuilder WithSkipWhen(Func<IModuleContext, SkipDecision> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);
        _skipConditions.Add((context, _) => ValueTask.FromResult(condition(context)));
        return this;
    }

    /// <summary>
    /// Adds an asynchronous skip condition.
    /// </summary>
    /// <param name="condition">A function that receives the module context and cancellation token and returns a <see cref="SkipDecision"/>.</param>
    /// <returns>This builder instance for method chaining.</returns>
    /// <remarks>Repeated conditions are combined with AND semantics and evaluated in registration order.</remarks>
    public ModuleConfigurationBuilder WithSkipWhen(
        Func<IModuleContext, CancellationToken, ValueTask<SkipDecision>> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);
        _skipConditions.Add(condition);
        return this;
    }

    #endregion

    #region Scheduling and Metadata

    /// <summary>
    /// Prevents this module from running in parallel with any other module.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithNotInParallel()
    {
        _parallelConstraintKeys = [];
        return this;
    }

    /// <summary>
    /// Prevents this module from running in parallel with modules using any matching constraint key.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithNotInParallel(params string[] constraintKeys)
    {
        ArgumentNullException.ThrowIfNull(constraintKeys);

        if (constraintKeys.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException("Constraint keys cannot be empty or whitespace.", nameof(constraintKeys));
        }

        if (constraintKeys.Length != constraintKeys.Distinct(StringComparer.Ordinal).Count())
        {
            throw new ArgumentException("Duplicate constraint keys are not allowed.", nameof(constraintKeys));
        }

        _parallelConstraintKeys = [.. constraintKeys];
        return this;
    }

    /// <summary>
    /// Sets the module scheduling priority.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithPriority(ModulePriority priority)
    {
        _priority = priority;
        return this;
    }

    /// <summary>
    /// Sets the module resource-usage hint.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithExecutionHint(ExecutionType executionType)
    {
        _executionType = executionType;
        return this;
    }

    /// <summary>
    /// Adds metadata tags to the module.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithTags(params string[] tags)
    {
        ArgumentNullException.ThrowIfNull(tags);

        if (tags.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException("Tags cannot be empty or whitespace.", nameof(tags));
        }

        _tags.UnionWith(tags);
        return this;
    }

    /// <summary>
    /// Sets the module category.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithCategory(string category)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(category);
        _category = category;
        return this;
    }

    #endregion

    #region Dependencies

    /// <summary>
    /// Adds a required dependency. The dependency must be registered with the pipeline.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder DependsOn<TModule>()
        where TModule : IModule
        => DependsOn(typeof(TModule));

    /// <summary>
    /// Adds a required dependency. The dependency must be registered with the pipeline.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder DependsOn(Type moduleType)
    {
        ValidateModuleType(moduleType);
        _dependencies.Add(DeclaredDependency.Required(moduleType));
        return this;
    }

    /// <summary>
    /// Adds an optional dependency.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder DependsOnOptional<TModule>()
        where TModule : IModule
        => DependsOnOptional(typeof(TModule));

    /// <summary>
    /// Adds an optional dependency.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder DependsOnOptional(Type moduleType)
    {
        ValidateModuleType(moduleType);
        _dependencies.Add(DeclaredDependency.Optional(moduleType));
        return this;
    }

    /// <summary>
    /// Adds a required dependency when the supplied condition is true.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder DependsOnIf<TModule>(bool condition)
        where TModule : IModule
        => condition ? DependsOn<TModule>() : this;

    /// <summary>
    /// Adds a required dependency when the supplied predicate returns true.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder DependsOnIf<TModule>(Func<bool> predicate)
        where TModule : IModule
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return DependsOnIf<TModule>(predicate());
    }

    /// <summary>
    /// Adds a required dependency when the supplied condition is true.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder DependsOnIf(Type moduleType, bool condition)
        => condition ? DependsOn(moduleType) : this;

    /// <summary>
    /// Adds a required dependency when the supplied predicate returns true.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder DependsOnIf(Type moduleType, Func<bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return DependsOnIf(moduleType, predicate());
    }

    /// <summary>
    /// Adds a lazy optional dependency.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder DependsOnLazy<TModule>()
        where TModule : IModule
    {
        _dependencies.Add(DeclaredDependency.Lazy(typeof(TModule)));
        return this;
    }

    /// <summary>
    /// Adds a lazy optional dependency.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder DependsOnLazy(Type moduleType)
    {
        ValidateModuleType(moduleType);
        _dependencies.Add(DeclaredDependency.Lazy(moduleType));
        return this;
    }

    #endregion

    #region WithTimeout

    /// <summary>
    /// Sets the timeout duration for module execution.
    /// </summary>
    /// <param name="timeout">The maximum duration allowed for module execution.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithTimeout(TimeSpan timeout)
    {
        _timeout = timeout;
        return this;
    }

    #endregion

    #region WithRetryPolicy Overloads

    /// <summary>
    /// Sets a retry policy for module execution.
    /// </summary>
    /// <param name="policy">The Polly async policy to use for retries.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithRetryPolicy(IAsyncPolicy policy)
    {
        _retryPolicyFactory = _ => policy;
        return this;
    }

    /// <summary>
    /// Sets a retry policy factory that creates the policy based on the module context.
    /// </summary>
    /// <param name="factory">A factory function that creates a Polly async policy using the module context.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithRetryPolicy(Func<IModuleContext, IAsyncPolicy> factory)
    {
        _retryPolicyFactory = factory;
        return this;
    }

    /// <summary>
    /// Sets a simple retry count using an exponential backoff strategy.
    /// </summary>
    /// <param name="count">The number of retry attempts.</param>
    /// <returns>This builder instance for method chaining.</returns>
    /// <remarks>
    /// This creates a retry policy with exponential backoff where each retry waits
    /// (attempt^2 * 100) milliseconds before the next attempt.
    /// </remarks>
    public ModuleConfigurationBuilder WithRetryCount(int count)
    {
        _retryPolicyFactory = _ => Policy.Handle<Exception>()
            .WaitAndRetryAsync(count, attempt => TimeSpan.FromMilliseconds(attempt * attempt * 100));
        return this;
    }

    #endregion

    #region WithIgnoreFailures Overloads

    /// <summary>
    /// Configures the module to always ignore failures.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithIgnoreFailures()
    {
        _ignoreFailuresCondition = (_, _) => Task.FromResult(true);
        return this;
    }

    /// <summary>
    /// Configures the module to ignore failures based on a synchronous condition.
    /// </summary>
    /// <param name="condition">A function that takes the module context and exception, returning true if the failure should be ignored.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithIgnoreFailuresWhen(Func<IModuleContext, Exception, bool> condition)
    {
        _ignoreFailuresCondition = (ctx, ex) => Task.FromResult(condition(ctx, ex));
        return this;
    }

    /// <summary>
    /// Configures the module to ignore failures based on an asynchronous condition.
    /// </summary>
    /// <param name="condition">An async function that takes the module context and exception, returning true if the failure should be ignored.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithIgnoreFailuresWhen(Func<IModuleContext, Exception, Task<bool>> condition)
    {
        _ignoreFailuresCondition = condition;
        return this;
    }

    #endregion

    #region WithAlwaysRun

    /// <summary>
    /// Configures the module to always run, even if other modules have failed.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithAlwaysRun()
    {
        _alwaysRun = true;
        return this;
    }

    #endregion

    /// <summary>
    /// Builds the <see cref="ModuleConfiguration"/> instance with the configured settings.
    /// </summary>
    /// <returns>A new <see cref="ModuleConfiguration"/> instance.</returns>
    public ModuleConfiguration Build()
    {
        return new ModuleConfiguration
        {
            SkipCondition = ComposeSkipConditions(),
            Timeout = _timeout,
            RetryPolicyFactory = _retryPolicyFactory,
            IgnoreFailuresCondition = _ignoreFailuresCondition,
            AlwaysRun = _alwaysRun,
            ParallelConstraintKeys = _parallelConstraintKeys,
            Priority = _priority,
            ExecutionType = _executionType,
            Tags = new HashSet<string>(_tags, StringComparer.OrdinalIgnoreCase),
            Category = _category,
            Dependencies = [.. _dependencies],
        };
    }

    private Func<IModuleContext, CancellationToken, ValueTask<SkipDecision>>? ComposeSkipConditions()
    {
        if (_skipConditions.Count == 0)
        {
            return null;
        }

        var conditions = _skipConditions.ToArray();
        return async (context, cancellationToken) =>
        {
            List<string>? reasons = null;

            foreach (var condition in conditions)
            {
                var decision = await condition(context, cancellationToken).ConfigureAwait(false);
                if (!decision.ShouldSkip)
                {
                    return SkipDecision.DoNotSkip;
                }

                if (!string.IsNullOrWhiteSpace(decision.Reason))
                {
                    (reasons ??= []).Add(decision.Reason);
                }
            }

            return SkipDecision.Skip(reasons is null ? null : string.Join("; ", reasons));
        };
    }

    private static void ValidateModuleType(Type moduleType)
    {
        ArgumentNullException.ThrowIfNull(moduleType);

        if (!moduleType.IsAssignableTo(typeof(IModule)))
        {
            throw new InvalidModuleTypeException(moduleType);
        }
    }
}
