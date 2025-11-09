using ModularPipelines.Context;
using ModularPipelines.Enums;

namespace ModularPipelines.Modules;

/// <summary>
/// The minimal contract for a pipeline module.
/// All modules must implement this interface.
/// </summary>
public interface IModule
{
    /// <summary>
    /// Gets the unique identifier for this module instance.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets the module's type (used for dependency resolution and identification).
    /// </summary>
    Type ModuleType { get; }

    /// <summary>
    /// Gets the current execution status of this module.
    /// </summary>
    Status Status { get; }

    /// <summary>
    /// Gets the exception that occurred during module execution, if any.
    /// </summary>
    Exception? Exception { get; }

    /// <summary>
    /// Gets the time when this module started execution.
    /// </summary>
    DateTimeOffset? StartTime { get; }

    /// <summary>
    /// Gets the time when this module completed execution.
    /// </summary>
    DateTimeOffset? EndTime { get; }
}

/// <summary>
/// Internal interface for setting module state properties.
/// This allows the pipeline engine to update module state while keeping setters internal.
/// </summary>
internal interface IModuleInternal
{
    /// <summary>
    /// Sets the current execution status of this module.
    /// </summary>
    Status Status { get; set; }

    /// <summary>
    /// Sets the exception that occurred during module execution.
    /// </summary>
    Exception? Exception { get; set; }

    /// <summary>
    /// Sets the time when this module started execution.
    /// </summary>
    DateTimeOffset? StartTime { get; set; }

    /// <summary>
    /// Sets the time when this module completed execution.
    /// </summary>
    DateTimeOffset? EndTime { get; set; }
}

/// <summary>
/// A module that produces a result of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of result this module produces.</typeparam>
public interface IModule<T> : IModule
{
    /// <summary>
    /// Execute the module's business logic.
    /// </summary>
    /// <param name="context">The pipeline context providing access to services and tools.</param>
    /// <param name="cancellationToken">A token that will be cancelled if the pipeline fails or module times out.</param>
    /// <returns>The result of the module execution, or null if no result is produced.</returns>
    Task<T?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken);
}
