using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Models;

namespace ModularPipelines.Modules;

/// <summary>
/// Base implementation of <see cref="IModule{T}"/> providing minimal functionality.
/// Inherit from this class to create custom pipeline modules.
/// </summary>
/// <typeparam name="T">The type of result this module produces.</typeparam>
public abstract class Module<T> : IModule<T>, IModuleInternal
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Module{T}"/> class.
    /// </summary>
    protected Module()
    {
        Id = Guid.NewGuid();
        SkipDecision = SkipDecision.DoNotSkip;
        Status = Status.NotYetStarted;
    }

    /// <inheritdoc />
    public Guid Id { get; }

    /// <inheritdoc />
    public Type ModuleType => GetType();

    /// <summary>
    /// Gets the cached result value from the module's execution.
    /// This is set by the module executor after ExecuteAsync completes.
    /// </summary>
    public T? Value { get; internal set; }

    /// <summary>
    /// Gets the skip decision for this module.
    /// This is set by the module executor when evaluating skip logic.
    /// </summary>
    public SkipDecision SkipDecision { get; internal set; }

    /// <summary>
    /// Gets the current execution status of this module.
    /// This is updated by the pipeline engine during execution.
    /// </summary>
    public Status Status { get; set; }

    /// <summary>
    /// Gets the time when this module started execution.
    /// </summary>
    public DateTimeOffset? StartTime { get; set; }

    /// <summary>
    /// Gets the time when this module completed execution.
    /// </summary>
    public DateTimeOffset? EndTime { get; set; }

    /// <summary>
    /// Gets the exception that occurred during module execution, if any.
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    /// Gets the duration of module execution.
    /// Returns null if the module hasn't started or completed yet.
    /// </summary>
    public TimeSpan? Duration
    {
        get
        {
            if (StartTime.HasValue && EndTime.HasValue)
            {
                return EndTime.Value - StartTime.Value;
            }
            return null;
        }
    }

    /// <summary>
    /// Attempts to cancel this module's execution.
    /// This is a no-op in the composition-based architecture as cancellation is managed by services.
    /// </summary>
    internal void TryCancel()
    {
        // No-op for Module - cancellation handled by execution services
    }

    /// <summary>
    /// Gets the module result asynchronously.
    /// Returns a completed task with a ModuleResult wrapping this module's value.
    /// </summary>
    internal Task<IModuleResult> GetModuleResult()
    {
        IModule module = this;
        return Task.FromResult<IModuleResult>(new ModuleResult<T>(Value, module));
    }

    /// <inheritdoc />
    public abstract Task<T?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken);

    /// <summary>
    /// Executes a named sub-task within this module with async function returning a result.
    /// SubModules provide progress tracking and clear logging for work done in loops or parallel operations.
    /// </summary>
    /// <typeparam name="TResult">The type of result the sub-task produces.</typeparam>
    /// <param name="context">The pipeline context.</param>
    /// <param name="name">The name of the sub-task for logging and progress tracking.</param>
    /// <param name="action">The async function to execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The result of the sub-task.</returns>
    protected async Task<TResult> SubModule<TResult>(
        IPipelineContext context,
        string name,
        Func<Task<TResult>> action,
        CancellationToken cancellationToken = default)
    {
        context.Logger.LogDebug("[SubModule] {ModuleName}.{SubModuleName} starting", ModuleType.Name, name);

        try
        {
            var result = await action();
            context.Logger.LogDebug("[SubModule] {ModuleName}.{SubModuleName} completed", ModuleType.Name, name);
            return result;
        }
        catch (Exception ex)
        {
            context.Logger.LogError(ex, "[SubModule] {ModuleName}.{SubModuleName} failed", ModuleType.Name, name);
            throw;
        }
    }

    /// <summary>
    /// Executes a named sub-task within this module with sync function returning a result.
    /// </summary>
    protected Task<TResult> SubModule<TResult>(
        IPipelineContext context,
        string name,
        Func<TResult> action,
        CancellationToken cancellationToken = default)
    {
        return SubModule(context, name, () => Task.FromResult(action()), cancellationToken);
    }

    /// <summary>
    /// Executes a named sub-task within this module with async function returning no result.
    /// </summary>
    protected async Task SubModule(
        IPipelineContext context,
        string name,
        Func<Task> action,
        CancellationToken cancellationToken = default)
    {
        context.Logger.LogDebug("[SubModule] {ModuleName}.{SubModuleName} starting", ModuleType.Name, name);

        try
        {
            await action();
            context.Logger.LogDebug("[SubModule] {ModuleName}.{SubModuleName} completed", ModuleType.Name, name);
        }
        catch (Exception ex)
        {
            context.Logger.LogError(ex, "[SubModule] {ModuleName}.{SubModuleName} failed", ModuleType.Name, name);
            throw;
        }
    }

    /// <summary>
    /// Executes a named sub-task within this module with sync action returning no result.
    /// </summary>
    protected Task SubModule(
        IPipelineContext context,
        string name,
        Action action,
        CancellationToken cancellationToken = default)
    {
        return SubModule(context, name, () =>
        {
            action();
            return Task.CompletedTask;
        }, cancellationToken);
    }
}

/// <summary>
/// Non-generic module base class that returns a dictionary.
/// Use this when you don't have a specific return type.
/// </summary>
public abstract class Module : Module<IDictionary<string, object>>
{
}
