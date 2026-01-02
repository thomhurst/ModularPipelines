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
}
