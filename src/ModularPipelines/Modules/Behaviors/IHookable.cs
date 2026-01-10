using ModularPipelines.Context;

namespace ModularPipelines.Modules.Behaviors;

/// <summary>
/// Implement this interface to execute code before and/or after module execution.
/// </summary>
/// <remarks>
/// <para>
/// <b>Execution Order:</b> Hooks run AFTER skip checks. If a module is skipped,
/// neither <see cref="OnBeforeExecute"/> nor <see cref="OnAfterExecute"/> are called.
/// </para>
/// <para>
/// <b>Failure Handling:</b> <see cref="OnAfterExecute"/> runs in a finally block,
/// so it executes regardless of whether the module succeeded or failed (but NOT if skipped).
/// </para>
/// <para>
/// For pipeline-wide hooks that run for all modules, use <see cref="Interfaces.IPipelineModuleHooks"/> instead.
/// </para>
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
