using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Http;
using ModularPipelines.Logging;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// The pipeline context, with access to configuration, DI and helper objects.
/// </summary>
/// <remarks>
/// This interface serves as a facade providing unified access to all pipeline capabilities.
/// Each property represents a focused helper for a specific domain (file system, commands, serialization, etc.).
///
/// The facade pattern is intentional here to:
/// - Provide a single entry point for all pipeline operations
/// - Allow dependency injection of all helpers
/// - Support extension methods for tool-specific integrations (Git, Docker, Azure, etc.)
///
/// Extension Pattern:
/// Tool integrations (ModularPipelines.Git, ModularPipelines.Docker, etc.) add extension methods
/// like context.Git() and context.Docker() that provide strongly-typed APIs for those tools.
///
/// Interface Segregation:
/// This interface inherits from focused sub-interfaces (IPipelineServices, IPipelineLogging, etc.)
/// allowing consumers to depend on only the subset of functionality they need.
/// </remarks>
public interface IPipelineHookContext :
    IPipelineServices,
    IPipelineLogging,
    IPipelineTools,
    IPipelineEncoding,
    IPipelineFileSystem,
    IPipelineEnvironment
{
    #region Internal

    /// <summary>
    /// Gets the detector used to detect modules which depend on each other.
    /// </summary>
    internal IDependencyCollisionDetector DependencyCollisionDetector { get; }

    /// <summary>
    /// Gets the results repository used for storing module results.
    /// </summary>
    internal IModuleResultRepository ModuleResultRepository { get; }

    /// <summary>
    /// Used to initialise the logger for this context.
    /// </summary>
    /// <param name="getType">The module type.</param>
    internal void InitializeLogger(Type getType);

    /// <summary>
    /// Gets the cancellation token used for cancelling the pipeline on failures.
    /// </summary>
    internal EngineCancellationToken EngineCancellationToken { get; }

    #endregion
}
