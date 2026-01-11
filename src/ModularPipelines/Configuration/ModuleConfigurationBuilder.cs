using ModularPipelines.Context;
using ModularPipelines.Models;
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
#pragma warning disable CS0618 // IPipelineContext is obsolete but we still need to support it
    private Func<IPipelineContext, Task<SkipDecision>>? _skipCondition;
    private TimeSpan? _timeout;
    private Func<IPipelineContext, IAsyncPolicy>? _retryPolicyFactory;
    private Func<IPipelineContext, Exception, Task<bool>>? _ignoreFailuresCondition;
    private bool _alwaysRun;
    private Func<IPipelineContext, Task>? _onBeforeExecute;
    private Func<IPipelineContext, Task>? _onAfterExecute;
#pragma warning restore CS0618

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
            return Task.FromResult<SkipDecision>(shouldSkip);
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
            return shouldSkip;
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
    /// Sets a skip condition based on a synchronous boolean function that receives the pipeline context.
    /// </summary>
    /// <param name="condition">A function that takes the pipeline context and returns true if the module should be skipped.</param>
    /// <returns>This builder instance for method chaining.</returns>
#pragma warning disable CS0618 // IPipelineContext is obsolete but we still need to support it
    public ModuleConfigurationBuilder WithSkipWhen(Func<IPipelineContext, bool> condition)
#pragma warning restore CS0618
    {
        _skipCondition = ctx =>
        {
            var shouldSkip = condition(ctx);
            return Task.FromResult<SkipDecision>(shouldSkip);
        };
        return this;
    }

    /// <summary>
    /// Sets a skip condition based on an asynchronous boolean function that receives the pipeline context.
    /// </summary>
    /// <param name="condition">An async function that takes the pipeline context and returns true if the module should be skipped.</param>
    /// <returns>This builder instance for method chaining.</returns>
#pragma warning disable CS0618 // IPipelineContext is obsolete but we still need to support it
    public ModuleConfigurationBuilder WithSkipWhen(Func<IPipelineContext, Task<bool>> condition)
#pragma warning restore CS0618
    {
        _skipCondition = async ctx =>
        {
            var shouldSkip = await condition(ctx).ConfigureAwait(false);
            return shouldSkip;
        };
        return this;
    }

    /// <summary>
    /// Sets a skip condition based on a synchronous function that receives the pipeline context and returns a <see cref="SkipDecision"/>.
    /// </summary>
    /// <param name="condition">A function that takes the pipeline context and returns a <see cref="SkipDecision"/>.</param>
    /// <returns>This builder instance for method chaining.</returns>
#pragma warning disable CS0618 // IPipelineContext is obsolete but we still need to support it
    public ModuleConfigurationBuilder WithSkipWhen(Func<IPipelineContext, SkipDecision> condition)
#pragma warning restore CS0618
    {
        _skipCondition = ctx => Task.FromResult(condition(ctx));
        return this;
    }

    /// <summary>
    /// Sets a skip condition based on an asynchronous function that receives the pipeline context and returns a <see cref="SkipDecision"/>.
    /// </summary>
    /// <param name="condition">An async function that takes the pipeline context and returns a <see cref="SkipDecision"/>.</param>
    /// <returns>This builder instance for method chaining.</returns>
#pragma warning disable CS0618 // IPipelineContext is obsolete but we still need to support it
    public ModuleConfigurationBuilder WithSkipWhen(Func<IPipelineContext, Task<SkipDecision>> condition)
#pragma warning restore CS0618
    {
        _skipCondition = condition;
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
    /// Sets a retry policy factory that creates the policy based on the pipeline context.
    /// </summary>
    /// <param name="factory">A factory function that creates a Polly async policy using the pipeline context.</param>
    /// <returns>This builder instance for method chaining.</returns>
#pragma warning disable CS0618 // IPipelineContext is obsolete but we still need to support it
    public ModuleConfigurationBuilder WithRetryPolicy(Func<IPipelineContext, IAsyncPolicy> factory)
#pragma warning restore CS0618
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
    /// <param name="condition">A function that takes the pipeline context and exception, returning true if the failure should be ignored.</param>
    /// <returns>This builder instance for method chaining.</returns>
#pragma warning disable CS0618 // IPipelineContext is obsolete but we still need to support it
    public ModuleConfigurationBuilder WithIgnoreFailuresWhen(Func<IPipelineContext, Exception, bool> condition)
#pragma warning restore CS0618
    {
        _ignoreFailuresCondition = (ctx, ex) => Task.FromResult(condition(ctx, ex));
        return this;
    }

    /// <summary>
    /// Configures the module to ignore failures based on an asynchronous condition.
    /// </summary>
    /// <param name="condition">An async function that takes the pipeline context and exception, returning true if the failure should be ignored.</param>
    /// <returns>This builder instance for method chaining.</returns>
#pragma warning disable CS0618 // IPipelineContext is obsolete but we still need to support it
    public ModuleConfigurationBuilder WithIgnoreFailuresWhen(Func<IPipelineContext, Exception, Task<bool>> condition)
#pragma warning restore CS0618
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
    /// <param name="hook">An async function that receives the pipeline context.</param>
    /// <returns>This builder instance for method chaining.</returns>
#pragma warning disable CS0618 // IPipelineContext is obsolete but we still need to support it
    public ModuleConfigurationBuilder WithBeforeExecute(Func<IPipelineContext, Task> hook)
#pragma warning restore CS0618
    {
        _onBeforeExecute = hook;
        return this;
    }

    /// <summary>
    /// Sets a hook to execute after the module's main execution.
    /// </summary>
    /// <param name="hook">An async function that receives the pipeline context.</param>
    /// <returns>This builder instance for method chaining.</returns>
#pragma warning disable CS0618 // IPipelineContext is obsolete but we still need to support it
    public ModuleConfigurationBuilder WithAfterExecute(Func<IPipelineContext, Task> hook)
#pragma warning restore CS0618
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
        };
    }
}
