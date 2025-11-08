using ModularPipelines.Context;
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
