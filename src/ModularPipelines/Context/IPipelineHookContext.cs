namespace ModularPipelines.Context;

/// <summary>
/// The pipeline context, with access to configuration, DI and helper objects.
/// </summary>
/// <remarks>
/// <para>
/// This interface serves as a facade providing unified access to all pipeline capabilities.
/// Each property represents a focused helper for a specific domain (file system, commands, serialization, etc.).
/// </para>
/// <para>
/// The facade pattern is intentional here to:
/// </para>
/// <list type="bullet">
/// <item><description>Provide a single entry point for all pipeline operations</description></item>
/// <item><description>Allow dependency injection of all helpers</description></item>
/// <item><description>Support extension methods for tool-specific integrations (Git, Docker, Azure, etc.)</description></item>
/// </list>
/// <para>
/// <b>Extension Pattern:</b>
/// Tool integrations (ModularPipelines.Git, ModularPipelines.Docker, etc.) add extension methods
/// like context.Git() and context.Docker() that provide strongly-typed APIs for those tools.
/// </para>
/// <para>
/// <b>Interface Segregation:</b>
/// This interface inherits from focused sub-interfaces (IPipelineServices, IPipelineLogging, etc.)
/// allowing consumers to depend on only the subset of functionality they need.
/// </para>
/// <para><b>Thread Safety:</b></para>
/// <para>
/// <see cref="IPipelineHookContext"/> and all its inherited interfaces are designed for thread-safe access.
/// The context can be safely shared across multiple threads within hooks and modules.
/// See individual sub-interfaces for specific thread-safety details.
/// </para>
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
