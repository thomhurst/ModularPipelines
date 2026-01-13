namespace ModularPipelines.Context;

/// <summary>
/// Context provided to pipeline-level hooks (Start/End).
/// </summary>
/// <remarks>
/// <para>
/// This interface extends <see cref="IPipelineContext"/> for use in pipeline hooks.
/// Pipeline hooks execute at the start and end of the entire pipeline run.
/// </para>
/// <para>
/// <b>Extension Pattern:</b>
/// Tool integrations (ModularPipelines.Git, ModularPipelines.Docker, etc.) add extension methods
/// like context.Git() and context.Docker() that provide strongly-typed APIs for those tools.
/// </para>
/// <para><b>Thread Safety:</b></para>
/// <para>
/// <see cref="IPipelineHookContext"/> and all its inherited interfaces are designed for thread-safe access.
/// The context can be safely shared across multiple threads within hooks and modules.
/// See <see cref="IPipelineContext"/> for specific thread-safety details.
/// </para>
/// </remarks>
public interface IPipelineHookContext : IPipelineContext
{
}
