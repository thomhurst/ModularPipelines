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
/// skip conditions, timeouts, retry policies, failure handling, and execution hooks.
/// </para>
/// <para>
/// All methods return the builder instance to support method chaining.
/// Call <see cref="Build"/> to create the final <see cref="ModuleConfiguration"/> instance.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var config = ModuleConfiguration.Create()
///     .WithSkipWhen(() => someCondition)
///     .WithTimeout(TimeSpan.FromMinutes(5))
///     .WithRetryCount(3)
///     .WithIgnoreFailures()
///     .WithAlwaysRun()
///     .WithBeforeExecute(ctx => LogStartAsync(ctx))
///     .WithAfterExecute(ctx => LogEndAsync(ctx))
///     .Build();
/// </code>
/// </example>
public sealed class ModuleConfigurationBuilder
{
    private readonly HashSet<string> _tags = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<DeclaredDependency> _dependencies = [];
    private Func<IModuleContext, Task<SkipDecision>>? _skipCondition;
    private TimeSpan? _timeout;
    private Func<IModuleContext, IAsyncPolicy>? _retryPolicyFactory;
    private Func<IModuleContext, Exception, Task<bool>>? _ignoreFailuresCondition;
    private bool _alwaysRun;
    private Func<IModuleContext, Task>? _onBeforeExecute;
    private Func<IModuleContext, Task>? _onAfterExecute;
    private string[]? _parallelConstraintKeys;
    private ModulePriority? _priority;
    private ExecutionType? _executionType;
    private string? _category;

    #region WithSkipWhen Overloads

    /// <summary>
    /// Sets a skip condition based on a synchronous boolean function.
    /// </summary>
    /// <param name="condition">A function that returns true if the module should be skipped.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithSkipWhen(Func<bool> condition)
    {
        _skipCondition = _ =>
        {
            var shouldSkip = condition();
            return Task.FromResult(SkipDecision.Of(shouldSkip, "Configured skip condition returned true"));
        };
        return this;
    }

    /// <summary>
    /// Sets a skip condition based on an asynchronous boolean function.
    /// </summary>
    /// <param name="condition">An async function that returns true if the module should be skipped.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithSkipWhen(Func<Task<bool>> condition)
    {
        _skipCondition = async _ =>
        {
            var shouldSkip = await condition().ConfigureAwait(false);
            return SkipDecision.Of(shouldSkip, "Configured skip condition returned true");
        };
        return this;
    }

    /// <summary>
    /// Sets a skip condition based on a synchronous function returning a <see cref="SkipDecision"/>.
    /// </summary>
    /// <param name="condition">A function that returns a <see cref="SkipDecision"/>.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithSkipWhen(Func<SkipDecision> condition)
    {
        _skipCondition = _ => Task.FromResult(condition());
        return this;
    }

    /// <summary>
    /// Sets a skip condition based on an asynchronous function returning a <see cref="SkipDecision"/>.
    /// </summary>
    /// <param name="condition">An async function that returns a <see cref="SkipDecision"/>.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithSkipWhen(Func<Task<SkipDecision>> condition)
    {
        _skipCondition = async _ => await condition().ConfigureAwait(false);
        return this;
    }

    /// <summary>
    /// Sets a skip condition based on a synchronous boolean function that receives the module context.
    /// </summary>
    /// <param name="condition">A function that takes the module context and returns true if the module should be skipped.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithSkipWhen(Func<IModuleContext, bool> condition)
    {
        _skipCondition = ctx =>
        {
            var shouldSkip = condition(ctx);
            return Task.FromResult(SkipDecision.Of(shouldSkip, "Configured skip condition returned true"));
        };
        return this;
    }

    /// <summary>
    /// Sets a skip condition based on an asynchronous boolean function that receives the module context.
    /// </summary>
    /// <param name="condition">An async function that takes the module context and returns true if the module should be skipped.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithSkipWhen(Func<IModuleContext, Task<bool>> condition)
    {
        _skipCondition = async ctx =>
        {
            var shouldSkip = await condition(ctx).ConfigureAwait(false);
            return SkipDecision.Of(shouldSkip, "Configured skip condition returned true");
        };
        return this;
    }

    /// <summary>
    /// Sets a skip condition based on a synchronous function that receives the module context and returns a <see cref="SkipDecision"/>.
    /// </summary>
    /// <param name="condition">A function that takes the module context and returns a <see cref="SkipDecision"/>.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithSkipWhen(Func<IModuleContext, SkipDecision> condition)
    {
        _skipCondition = ctx => Task.FromResult(condition(ctx));
        return this;
    }

    /// <summary>
    /// Sets a skip condition based on an asynchronous function that receives the module context and returns a <see cref="SkipDecision"/>.
    /// </summary>
    /// <param name="condition">An async function that takes the module context and returns a <see cref="SkipDecision"/>.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithSkipWhen(Func<IModuleContext, Task<SkipDecision>> condition)
    {
        _skipCondition = condition;
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

    #region Hooks

    /// <summary>
    /// Sets a hook to execute before the module's main execution.
    /// </summary>
    /// <param name="hook">An async function that receives the module context.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithBeforeExecute(Func<IModuleContext, Task> hook)
    {
        _onBeforeExecute = hook;
        return this;
    }

    /// <summary>
    /// Sets a hook to execute after the module's main execution.
    /// </summary>
    /// <param name="hook">An async function that receives the module context.</param>
    /// <returns>This builder instance for method chaining.</returns>
    public ModuleConfigurationBuilder WithAfterExecute(Func<IModuleContext, Task> hook)
    {
        _onAfterExecute = hook;
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
            SkipCondition = _skipCondition,
            Timeout = _timeout,
            RetryPolicyFactory = _retryPolicyFactory,
            IgnoreFailuresCondition = _ignoreFailuresCondition,
            AlwaysRun = _alwaysRun,
            OnBeforeExecute = _onBeforeExecute,
            OnAfterExecute = _onAfterExecute,
            ParallelConstraintKeys = _parallelConstraintKeys,
            Priority = _priority,
            ExecutionType = _executionType,
            Tags = new HashSet<string>(_tags, StringComparer.OrdinalIgnoreCase),
            Category = _category,
            Dependencies = [.. _dependencies],
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
