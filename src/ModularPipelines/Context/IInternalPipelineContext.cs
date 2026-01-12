using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;

namespace ModularPipelines.Context;

/// <summary>
/// Internal interface for pipeline context members that are used by the framework internally.
/// </summary>
/// <remarks>
/// This interface separates internal implementation details from the public API surface.
/// These members are used by the pipeline engine for dependency detection, result storage,
/// logger initialization, cancellation handling, and module retrieval.
/// </remarks>
internal interface IInternalPipelineContext
{
    /// <summary>
    /// Gets the detector used to detect modules which depend on each other.
    /// </summary>
    IDependencyCollisionDetector DependencyCollisionDetector { get; }

    /// <summary>
    /// Gets the results repository used for storing module results.
    /// </summary>
    IModuleResultRepository ModuleResultRepository { get; }

    /// <summary>
    /// Used to initialise the logger for this context.
    /// </summary>
    /// <param name="getType">The module type.</param>
    void InitializeLogger(Type getType);

    /// <summary>
    /// Gets the cancellation token used for cancelling the pipeline on failures.
    /// </summary>
    EngineCancellationToken EngineCancellationToken { get; }

    /// <summary>
    /// Gets a module by type.
    /// </summary>
    /// <typeparam name="TModule">The type of module to retrieve.</typeparam>
    /// <returns>The module instance, or null if not found.</returns>
    TModule? GetModule<TModule>()
        where TModule : class, IModule;

    /// <summary>
    /// Gets a module by type.
    /// </summary>
    /// <param name="type">The type of module to retrieve.</param>
    /// <returns>The module instance, or null if not found.</returns>
    IModule? GetModule(Type type);
}
