using ModularPipelines.Context;
using ModularPipelines.Engine;
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
///     .WithRetry(3)
///     .WithIgnoreFailures()
///     .WithAlwaysRun()
///     .Build();
/// </code>
/// </example>
public sealed class ModuleConfigurationBuilder
{
    private readonly HashSet<string> _tags = [with(StringComparer.OrdinalIgnoreCase)];
    private readonly List<DeclaredDependency> _dependencies = [];
    private readonly List<Func<IModuleContext, CancellationToken, ValueTask<SkipDecision>>> _skipConditions = [];
    private readonly List<Func<IModuleContext, CancellationToken, ValueTask<SkipDecision?>>> _planningSkipConditions = [];
    private readonly List<string> _cacheKeyParts = [];
    private readonly HashSet<string> _cacheEnvironmentVariables = [with(StringComparer.Ordinal)];
    private string? _cacheAssemblyVersionKey;
    private TimeSpan? _timeout;
    private ModuleRetryConfiguration? _retryConfiguration;
    private Func<IModuleContext, IAsyncPolicy>? _advancedRetryPolicyFactory;
    private Func<IModuleContext, Exception, Task<bool>>? _ignoreFailuresCondition;
    private bool _alwaysRun;
    private string[]? _parallelConstraintKeys;
    private ModulePriority? _priority;
    private ExecutionType? _executionType;
    private string? _category;

    /// <summary>
    /// Gets advanced configuration APIs that expose third-party policy abstractions.
    /// </summary>
    public AdvancedModuleConfigurationBuilder Advanced => new(this);

    #region WithSkipWhen Overloads

    /// <summary>
    /// Adds a synchronous skip condition.
    /// </summary>
    /// <param name="condition">A side-effect-free function that receives the module context and returns a <see cref="SkipDecision"/>. It may be evaluated while building a dry-run plan.</param>
    /// <returns>This builder instance for method chaining.</returns>
    /// <remarks>
    /// Repeated conditions use OR-to-skip semantics. Evaluation stops when a condition returns
    /// <see cref="SkipDecision.Skip(string?)"/>.
    /// </remarks>
    public ModuleConfigurationBuilder WithSkipWhen(Func<IModuleContext, SkipDecision> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);
        var adaptedCondition = AdaptSkipCondition(condition);
        _skipConditions.Add(adaptedCondition);
        _planningSkipConditions.Add(AdaptPlanningSkipCondition(adaptedCondition));
        return this;
    }

    /// <summary>
    /// Adds an asynchronous skip condition.
    /// </summary>
    /// <param name="condition">A side-effect-free function that receives the module context and cancellation token and returns a <see cref="SkipDecision"/>. It may be evaluated while building a dry-run plan.</param>
    /// <returns>This builder instance for method chaining.</returns>
    /// <remarks>
    /// Repeated conditions use OR-to-skip semantics. Evaluation stops when a condition returns
    /// <see cref="SkipDecision.Skip(string?)"/>.
    /// </remarks>
    public ModuleConfigurationBuilder WithSkipWhen(
        Func<IModuleContext, CancellationToken, ValueTask<SkipDecision>> condition)
    {
        ArgumentNullException.ThrowIfNull(condition);
        _skipConditions.Add(condition);
        _planningSkipConditions.Add(AdaptPlanningSkipCondition(condition));
        return this;
    }

    /// <summary>
    /// Adds a synchronous group of conditions that must all return skip decisions to skip the module.
    /// </summary>
    /// <param name="conditions">Side-effect-free conditions evaluated in registration order with AND-to-skip semantics. They may be evaluated while building a dry-run plan.</param>
    /// <returns>This builder instance for method chaining.</returns>
    /// <remarks>
    /// Each call adds one AND group. The group composes with other skip conditions using OR-to-skip semantics.
    /// </remarks>
    public ModuleConfigurationBuilder WithSkipWhenAll(
        params Func<IModuleContext, SkipDecision>[] conditions)
    {
        ArgumentNullException.ThrowIfNull(conditions);
        ValidateSkipConditionGroup(conditions);

        var adaptedConditions = Array.ConvertAll(conditions, AdaptSkipCondition);
        _skipConditions.Add(ComposeAllSkipConditions(adaptedConditions));
        _planningSkipConditions.Add(ComposeAllPlanningSkipConditions(adaptedConditions));
        return this;
    }

    /// <summary>
    /// Adds an asynchronous group of conditions that must all return skip decisions to skip the module.
    /// </summary>
    /// <param name="conditions">Side-effect-free conditions evaluated in registration order with AND-to-skip semantics. They may be evaluated while building a dry-run plan.</param>
    /// <returns>This builder instance for method chaining.</returns>
    /// <remarks>
    /// Each call adds one AND group. The group composes with other skip conditions using OR-to-skip semantics.
    /// </remarks>
    public ModuleConfigurationBuilder WithSkipWhenAll(
        params Func<IModuleContext, CancellationToken, ValueTask<SkipDecision>>[] conditions)
    {
        ArgumentNullException.ThrowIfNull(conditions);
        ValidateSkipConditionGroup(conditions);

        _skipConditions.Add(ComposeAllSkipConditions([.. conditions]));
        _planningSkipConditions.Add(ComposeAllPlanningSkipConditions(conditions));
        return this;
    }

    #endregion

    #region Caching

    /// <summary>
    /// Adds an explicit value to this module's cache fingerprint.
    /// </summary>
    /// <param name="value">A stable value representing configuration or another logical input.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithCacheKeyPart(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        _cacheKeyParts.Add(value);
        return this;
    }

    /// <summary>
    /// Includes the current value of an environment variable in this module's cache fingerprint.
    /// </summary>
    /// <param name="variableName">The environment variable name.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithCacheEnvironmentVariable(string variableName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(variableName);
        _cacheEnvironmentVariables.Add(variableName);
        return this;
    }

    /// <summary>
    /// Replaces the module assembly MVID in this module's cache fingerprint with an explicit stable key.
    /// </summary>
    /// <param name="value">A stable version key that changes whenever the module implementation changes.</param>
    /// <returns>This builder instance for method chaining.</returns>
    /// <remarks>
    /// Use this when build tooling changes the assembly MVID without changing module behavior.
    /// Reusing a key after changing the module implementation can restore stale cached results.
    /// </remarks>
    public ModuleConfigurationBuilder WithCacheAssemblyVersionKey(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        _cacheAssemblyVersionKey = value;
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
    /// Adds a required dependency when the supplied condition is true.
    /// </summary>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder DependsOnIf(Type moduleType, bool condition)
        => condition ? DependsOn(moduleType) : this;

    #endregion

    #region WithTimeout

    /// <summary>
    /// Sets the timeout duration for each module execution attempt.
    /// </summary>
    /// <param name="timeout">The maximum duration allowed for each execution attempt.</param>
    /// <returns>This builder instance for method chaining.</returns>
    /// <remarks>
    /// When retries are configured, the timeout restarts for every attempt and does not include retry delays.
    /// </remarks>
    public ModuleConfigurationBuilder WithTimeout(TimeSpan timeout)
    {
        _timeout = timeout;
        return this;
    }

    #endregion

    #region WithRetry

    /// <summary>
    /// Configures retries with exponential backoff and jitter.
    /// </summary>
    /// <param name="count">The number of retry attempts after the initial execution.</param>
    /// <param name="baseDelay">The first exponential-backoff ceiling. Defaults to 100 milliseconds.</param>
    /// <param name="shouldRetry">An optional predicate that selects retryable exceptions.</param>
    /// <returns>This builder instance for method chaining.</returns>
    /// <remarks>
    /// Each delay uses equal jitter between half and all of its exponential-backoff ceiling.
    /// A null <paramref name="shouldRetry"/> retries every exception handled by the retry engine.
    /// A configured module timeout applies separately to each attempt and does not include these delays.
    /// </remarks>
    public ModuleConfigurationBuilder WithRetry(
        int count,
        TimeSpan? baseDelay = null,
        Func<Exception, bool>? shouldRetry = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        var retryBaseDelay = baseDelay ?? ModuleRetryConfiguration.DefaultBaseDelay;
        if (retryBaseDelay < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(baseDelay), baseDelay, "The retry base delay cannot be negative.");
        }

        _retryConfiguration = new ModuleRetryConfiguration(count, retryBaseDelay, shouldRetry);
        _advancedRetryPolicyFactory = null;
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
            PlanningSkipCondition = ComposePlanningSkipConditions(),
            Timeout = _timeout,
            RetryConfiguration = _retryConfiguration,
            AdvancedRetryPolicyFactory = _advancedRetryPolicyFactory,
            IgnoreFailuresCondition = _ignoreFailuresCondition,
            AlwaysRun = _alwaysRun,
            ParallelConstraintKeys = _parallelConstraintKeys,
            Priority = _priority,
            ExecutionType = _executionType,
            Tags = new HashSet<string>(_tags, StringComparer.OrdinalIgnoreCase),
            Category = _category,
            CacheKeyParts = [.. _cacheKeyParts],
            CacheEnvironmentVariables = [.. _cacheEnvironmentVariables],
            CacheAssemblyVersionKey = _cacheAssemblyVersionKey,
            Dependencies = [.. _dependencies],
        };
    }

    internal ModuleConfigurationBuilder SetAdvancedRetryPolicy(Func<IModuleContext, IAsyncPolicy> factory)
    {
        _advancedRetryPolicyFactory = factory;
        _retryConfiguration = null;
        return this;
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
            foreach (var condition in conditions)
            {
                var decision = await condition(context, cancellationToken).ConfigureAwait(false);
                if (decision.ShouldSkip)
                {
                    return decision;
                }
            }

            return SkipDecision.DoNotSkip;
        };
    }

    private Func<IModuleContext, CancellationToken, ValueTask<SkipDecision?>>? ComposePlanningSkipConditions()
    {
        if (_planningSkipConditions.Count == 0)
        {
            return null;
        }

        var conditions = _planningSkipConditions.ToArray();
        return async (context, cancellationToken) =>
        {
            var hasUnknownDecision = false;
            foreach (var condition in conditions)
            {
                var decision = await condition(context, cancellationToken).ConfigureAwait(false);
                if (decision?.ShouldSkip is true)
                {
                    return decision;
                }

                hasUnknownDecision |= decision is null;
            }

            return hasUnknownDecision ? null : SkipDecision.DoNotSkip;
        };
    }

    private static Func<IModuleContext, CancellationToken, ValueTask<SkipDecision>> ComposeAllSkipConditions(
        IReadOnlyList<Func<IModuleContext, CancellationToken, ValueTask<SkipDecision>>> conditions)
    {
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

    private static Func<IModuleContext, CancellationToken, ValueTask<SkipDecision?>> ComposeAllPlanningSkipConditions(
        IReadOnlyList<Func<IModuleContext, CancellationToken, ValueTask<SkipDecision>>> conditions)
    {
        var planningConditions = conditions.Select(AdaptPlanningSkipCondition).ToArray();
        return async (context, cancellationToken) =>
        {
            List<string>? reasons = null;
            var hasUnknownDecision = false;

            foreach (var condition in planningConditions)
            {
                var decision = await condition(context, cancellationToken).ConfigureAwait(false);
                if (decision is null)
                {
                    hasUnknownDecision = true;
                    continue;
                }

                if (!decision.ShouldSkip)
                {
                    return SkipDecision.DoNotSkip;
                }

                if (!string.IsNullOrWhiteSpace(decision.Reason))
                {
                    (reasons ??= []).Add(decision.Reason);
                }
            }

            return hasUnknownDecision
                ? null
                : SkipDecision.Skip(reasons is null ? null : string.Join("; ", reasons));
        };
    }

    private static Func<IModuleContext, CancellationToken, ValueTask<SkipDecision?>> AdaptPlanningSkipCondition(
        Func<IModuleContext, CancellationToken, ValueTask<SkipDecision>> condition)
    {
        return async (context, cancellationToken) =>
        {
            try
            {
                return await condition(context, cancellationToken).ConfigureAwait(false);
            }
            catch (PlanningModuleResultUnavailableException)
            {
                return null;
            }
        };
    }

    private static Func<IModuleContext, CancellationToken, ValueTask<SkipDecision>> AdaptSkipCondition(
        Func<IModuleContext, SkipDecision> condition)
    {
        return (context, _) => ValueTask.FromResult(condition(context));
    }

    private static void ValidateSkipConditionGroup<TCondition>(TCondition[] conditions)
        where TCondition : Delegate
    {
        if (conditions.Length == 0)
        {
            throw new ArgumentException("At least one skip condition is required.", nameof(conditions));
        }

        if (conditions.Any(static condition => condition is null))
        {
            throw new ArgumentException("Skip conditions cannot contain null values.", nameof(conditions));
        }
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
