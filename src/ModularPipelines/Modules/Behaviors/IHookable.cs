using ModularPipelines.Context;

namespace ModularPipelines.Modules.Behaviors;

/// <summary>
/// Implement this interface to execute code before and/or after module execution.
/// </summary>
/// <remarks>
/// Hooks are executed even if the module is skipped or fails:
/// <list type="bullet">
/// <item><see cref="OnBeforeExecute"/> runs before skip checks</item>
/// <item><see cref="OnAfterExecute"/> runs in a finally block after execution completes</item>
/// </list>
/// For pipeline-wide hooks, use <see cref="Interfaces.IPipelineModuleHooks"/> instead.
/// </remarks>
public interface IHookable
{
    /// <summary>
    /// Executes before the module starts.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <returns>A task representing the hook operation.</returns>
    Task OnBeforeExecute(IPipelineContext context);

    /// <summary>
    /// Executes after the module completes, regardless of success or failure.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <returns>A task representing the hook operation.</returns>
    Task OnAfterExecute(IPipelineContext context);
}
