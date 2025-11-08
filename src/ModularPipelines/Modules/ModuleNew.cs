using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Models;

namespace ModularPipelines.Modules;

/// <summary>
/// Base implementation of <see cref="IModule{T}"/> providing minimal functionality.
/// Inherit from this class to create custom pipeline modules.
/// </summary>
/// <typeparam name="T">The type of result this module produces.</typeparam>
public abstract class ModuleNew<T> : IModule<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleNew{T}"/> class.
    /// </summary>
    protected ModuleNew()
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
    public Status Status { get; internal set; }

    /// <summary>
    /// Attempts to cancel this module's execution.
    /// This is a no-op in the composition-based architecture as cancellation is managed by services.
    /// </summary>
    internal void TryCancel()
    {
        // No-op for ModuleNew - cancellation handled by IModuleStateTracker service
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
}

/// <summary>
/// Non-generic module base class that returns a dictionary.
/// Use this when you don't have a specific return type.
/// </summary>
public abstract class ModuleNew : ModuleNew<IDictionary<string, object>>
{
}
