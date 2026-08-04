using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;

namespace ModularPipelines.Modules;

#pragma warning disable SA1202 // Lifecycle members are ordered by execution phase rather than accessibility.

/// <summary>
/// Core interface for pipeline modules. Implement this interface to define a module's execution logic.
/// </summary>
/// <typeparam name="T">The type of result returned by the module.</typeparam>
/// <remarks>
/// <para>
/// Modules can customize execution behavior by overriding <see cref="Configure"/>:
/// </para>
/// <list type="bullet">
/// <item><see cref="ModuleConfigurationBuilder.WithSkipWhen(System.Func{IModuleContext, SkipDecision})"/> - Define skip conditions</item>
/// <item><see cref="ModuleConfigurationBuilder.WithTimeout"/> - Set execution timeout</item>
/// <item><see cref="ModuleConfigurationBuilder.WithRetry"/> - Configure retries</item>
/// <item><see cref="ModuleConfigurationBuilder.WithIgnoreFailures()"/> - Handle failures gracefully</item>
/// <item><see cref="ModuleConfigurationBuilder.WithAlwaysRun"/> - Run even when pipeline fails</item>
/// <item><see cref="ModuleConfigurationBuilder.WithPriority"/> - Set scheduling priority</item>
/// <item><see cref="ModuleConfigurationBuilder.WithExecutionHint"/> - Select resource concurrency limits</item>
/// <item><see cref="ModuleConfigurationBuilder.WithNotInParallel()"/> - Configure parallel constraints</item>
/// <item><see cref="ModuleConfigurationBuilder.WithTags"/> and <see cref="ModuleConfigurationBuilder.WithCategory"/> - Add metadata</item>
/// <item><see cref="ModuleConfigurationBuilder.DependsOn{TModule}"/> - Declare dependencies</item>
/// </list>
/// <para>
/// Dependencies can be declared through <see cref="Configure"/> or statically via
/// <see cref="Attributes.DependsOnAttribute"/> attributes.
/// </para>
/// <para>
/// Inherit from the non-generic <see cref="Module"/> when the module does not return a value.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ApiModule : Module&lt;ApiResult&gt;
/// {
///     protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
///         .DependsOn&lt;DatabaseModule&gt;()
///         .DependsOnOptional&lt;CachingModule&gt;()
///         .DependsOnIf&lt;MonitoringModule&gt;(Environment.IsProduction)
///         .Build();
///
///     protected override Task&lt;ApiResult&gt; ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
///         =&gt; Task.FromResult(new ApiResult());
/// }
/// </code>
/// </example>
public abstract class Module<T> : IModule, IPlanningModuleCopyProvider
{
    private Lazy<ModuleConfiguration> _configuration;

    // Exposes hook-transformed results to self-awaits without completing the public result early.
    private AsyncLocal<ModuleResult<T>?> _provisionalResult = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="Module{T}"/> class.
    /// </summary>
    protected Module()
    {
        _configuration = CreateConfigurationLazy();
    }

    internal TaskCompletionSource<ModuleResult<T>> CompletionSource { get; private set; } =
        CreateCompletionSource();

    /// <inheritdoc />
    Task<IModuleResult> IModule.ResultTask
    {
        get
        {
            PlanningModuleResultAccess.ThrowIfUnavailable(GetType());
            if (_provisionalResult.Value is { } provisionalResult)
            {
                return Task.FromResult<IModuleResult>(provisionalResult);
            }

            return CompletionSource.Task.ContinueWith(
                static t => (IModuleResult) t.Result,
                TaskContinuationOptions.ExecuteSynchronously);
        }
    }

    /// <inheritdoc />
    Type IModule.ResultType => typeof(T);

    /// <summary>
    /// Override to configure module behaviors (skip, timeout, retry, etc.).
    /// </summary>
    /// <returns>The module configuration.</returns>
    /// <remarks>
    /// <para>
    /// This method is called once during module activation and cached for subsequent accesses.
    /// Directly constructed modules initialize on first access.
    /// </para>
    /// <para>
    /// Use <see cref="ModuleConfiguration.Create"/> to build a custom configuration,
    /// or return <see cref="ModuleConfiguration.Default"/> for default behavior.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
    ///     .WithTimeout(TimeSpan.FromMinutes(5))
    ///     .WithRetry(3)
    ///     .WithAlwaysRun()
    ///     .Build();
    /// </code>
    /// </example>
    protected virtual ModuleConfiguration Configure() => ModuleConfiguration.Default;

    /// <inheritdoc />
    ModuleConfiguration IModule.Configuration => _configuration.Value;

    IModule IPlanningModuleCopyProvider.CreatePlanningCopy(IServiceProvider serviceProvider) =>
        CreatePlanningCopy(serviceProvider);

    bool IPlanningModuleCopyProvider.IsConfigurationInitialized => _configuration.IsValueCreated;

    void IPlanningModuleCopyProvider.InitializeConfiguration() => _ = _configuration.Value;

    void IPlanningModuleCopyProvider.InitializeConfiguration(ModuleConfiguration configuration) =>
        _configuration = CreateInitializedConfigurationLazy(configuration);

    IModule IPlanningModuleCopyProvider.CreatePlanningCopyFromRegisteredInstance(
        bool preserveConfiguration)
    {
        var copy = (Module<T>) MemberwiseClone();
        copy._configuration = preserveConfiguration && _configuration.IsValueCreated
            ? CreateInitializedConfigurationLazy(_configuration.Value)
            : copy.CreateConfigurationLazy();
        copy._provisionalResult = new AsyncLocal<ModuleResult<T>?>();
        copy.CompletionSource = CreateCompletionSource();
        return copy;
    }

    /// <summary>
    /// Creates an isolated module instance for dependency-graph planning.
    /// </summary>
    /// <remarks>
    /// Override this when the module is registered with constructor values that cannot be resolved
    /// from dependency injection. The returned instance must not share mutable module-owned state.
    /// </remarks>
    /// <param name="serviceProvider">The pipeline service provider.</param>
    /// <returns>An isolated module instance used only for planning.</returns>
    protected virtual Module<T> CreatePlanningCopy(IServiceProvider serviceProvider)
    {
        return (Module<T>) serviceProvider
            .GetRequiredService<IModuleActivator>()
            .CreatePlanningModule(GetType(), serviceProvider);
    }

    /// <summary>
    /// Executes the module's core logic.
    /// </summary>
    /// <param name="context">The module context providing access to pipeline services.</param>
    /// <param name="cancellationToken">A token that will be cancelled if the pipeline fails or the module times out.</param>
    /// <returns>The result of the module execution.</returns>
    /// <remarks>
    /// This method is called by the pipeline engine. Do not call it directly.
    /// To access another module's result, use <c>await context.GetModule&lt;TModule&gt;()</c>.
    /// </remarks>
    protected internal abstract Task<T> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken);

    /// <summary>
    /// Called before the module executes. Override to add setup logic.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the operation.</returns>
    /// <remarks>
    /// <para>
    /// Exceptions thrown from this method will prevent <see cref="ExecuteAsync"/> from running
    /// and will be propagated as a module failure.
    /// </para>
    /// <para>
    /// <strong>Edge case:</strong> If <see cref="OnBeforeExecuteAsync"/> throws an exception,
    /// <see cref="OnFailedAsync"/> is called, but <see cref="OnAfterExecuteAsync"/> is not called
    /// because the before hook did not complete. Attribute failure handlers and registered
    /// <see cref="IModuleEventReceiver"/> implementations are still notified.
    /// </para>
    /// </remarks>
    protected virtual Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        => Task.CompletedTask;

    /// <summary>
    /// Called after the module completes (success or failure). Override to add cleanup or result transformation.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <param name="result">The module result (may contain an exception on failure).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A modified result, or null to keep the original.</returns>
    /// <remarks>
    /// <para>
    /// This hook is called after both successful execution and failures. On failure, it is called
    /// after <see cref="OnFailedAsync"/>. Check <see cref="ModuleResult.ExceptionOrDefault"/> to determine
    /// if the module failed.
    /// </para>
    /// <para>
    /// <strong>Edge case:</strong> If <see cref="OnBeforeExecuteAsync"/> throws an exception,
    /// <see cref="OnAfterExecuteAsync"/> will NOT be called because the module execution never started.
    /// </para>
    /// <para>
    /// Exceptions thrown from this hook are logged but do not affect the module result.
    /// </para>
    /// </remarks>
    protected virtual Task<ModuleResult<T>?> OnAfterExecuteAsync(
        IModuleContext context,
        ModuleResult<T> result,
        CancellationToken cancellationToken)
        => Task.FromResult<ModuleResult<T>?>(null);

    /// <summary>
    /// Called when the module is skipped. Override to add skip notification logic.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <param name="skipDecision">The skip decision with reason.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the operation.</returns>
    protected virtual Task OnSkippedAsync(
        IModuleContext context,
        SkipDecision skipDecision,
        CancellationToken cancellationToken)
        => Task.CompletedTask;

    /// <summary>
    /// Called when the module's result is restored from the fingerprint cache.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <param name="result">The cached module result.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the operation.</returns>
    protected virtual Task OnCachedResultAsync(
        IModuleContext context,
        ModuleResult<T> result,
        CancellationToken cancellationToken)
        => Task.CompletedTask;

    /// <summary>
    /// Called when the module fails with an exception. Override to add error handling.
    /// Called before OnAfterExecuteAsync.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the operation.</returns>
    /// <remarks>
    /// <para>
    /// This hook is called when <see cref="ExecuteAsync"/> throws an exception.
    /// </para>
    /// <para>
    /// This hook is also called when <see cref="OnBeforeExecuteAsync"/> throws.
    /// </para>
    /// <para>
    /// Exceptions thrown from this hook are logged but do not prevent <see cref="OnAfterExecuteAsync"/>
    /// from being called.
    /// </para>
    /// </remarks>
    protected virtual Task OnFailedAsync(
        IModuleContext context,
        Exception exception,
        CancellationToken cancellationToken)
        => Task.CompletedTask;

    /// <summary>
    /// Internal accessor to invoke OnBeforeExecuteAsync from the engine.
    /// </summary>
    internal Task InvokeOnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        => OnBeforeExecuteAsync(context, cancellationToken);

    /// <summary>
    /// Internal accessor to invoke OnAfterExecuteAsync from the engine.
    /// </summary>
    internal Task<ModuleResult<T>?> InvokeOnAfterExecuteAsync(
        IModuleContext context,
        ModuleResult<T> result,
        CancellationToken cancellationToken)
        => OnAfterExecuteAsync(context, result, cancellationToken);

    /// <summary>
    /// Internal accessor to invoke OnSkippedAsync from the engine.
    /// </summary>
    internal Task InvokeOnSkippedAsync(
        IModuleContext context,
        SkipDecision skipDecision,
        CancellationToken cancellationToken)
        => OnSkippedAsync(context, skipDecision, cancellationToken);

    /// <summary>
    /// Internal accessor to invoke OnCachedResultAsync from the engine.
    /// </summary>
    internal Task InvokeOnCachedResultAsync(
        IModuleContext context,
        ModuleResult<T> result,
        CancellationToken cancellationToken)
        => OnCachedResultAsync(context, result, cancellationToken);

    /// <summary>
    /// Internal accessor to invoke OnFailedAsync from the engine.
    /// </summary>
    internal Task InvokeOnFailedAsync(
        IModuleContext context,
        Exception exception,
        CancellationToken cancellationToken)
        => OnFailedAsync(context, exception, cancellationToken);

    /// <inheritdoc />
    bool IModule.TrySetDistributedResult(IModuleResult result)
    {
        return CompletionSource.TrySetResult((ModuleResult<T>) result);
    }

    internal ModuleResult<T>? SetProvisionalResult(ModuleResult<T> result)
    {
        var previousResult = _provisionalResult.Value;
        _provisionalResult.Value = result;
        return previousResult;
    }

    internal void RestoreProvisionalResult(ModuleResult<T>? result)
    {
        _provisionalResult.Value = result;
    }

    /// <summary>
    /// Gets an awaiter for this module's result.
    /// </summary>
    /// <returns>An awaiter for the module result.</returns>
    public TaskAwaiter<ModuleResult<T>> GetAwaiter()
    {
        PlanningModuleResultAccess.ThrowIfUnavailable(GetType());
        return _provisionalResult.Value is { } provisionalResult
            ? Task.FromResult(provisionalResult).GetAwaiter()
            : CompletionSource.Task.GetAwaiter();
    }

    private ModuleConfiguration CreateConfiguration() =>
        ModuleConfigurationAttributeAdapter.Apply(
            GetType(),
            Configure());

    private Lazy<ModuleConfiguration> CreateConfigurationLazy() =>
        new(CreateConfiguration, LazyThreadSafetyMode.ExecutionAndPublication);

    private static Lazy<ModuleConfiguration> CreateInitializedConfigurationLazy(
        ModuleConfiguration configuration) =>
        new(() => configuration, LazyThreadSafetyMode.ExecutionAndPublication);

    private static TaskCompletionSource<ModuleResult<T>> CreateCompletionSource() =>
        new(TaskCreationOptions.RunContinuationsAsynchronously);
}

internal interface IPlanningModuleCopyProvider
{
    bool IsConfigurationInitialized { get; }

    IModule CreatePlanningCopy(IServiceProvider serviceProvider);

    IModule CreatePlanningCopyFromRegisteredInstance(bool preserveConfiguration);

    void InitializeConfiguration();

    void InitializeConfiguration(ModuleConfiguration configuration);
}

#pragma warning restore SA1202
