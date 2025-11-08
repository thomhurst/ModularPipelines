using ModularPipelines.Modules;

namespace ModularPipelines.Services;

/// <summary>
/// Service responsible for executing sub-modules with progress tracking.
/// This replaces the SubModule methods previously embedded in ModuleBase.
/// </summary>
public interface IModuleSubModuleService
{
    /// <summary>
    /// Executes a sub-module with progress tracking.
    /// </summary>
    /// <typeparam name="TResult">The type of result the sub-module produces.</typeparam>
    /// <param name="parentModule">The parent module that owns this sub-module.</param>
    /// <param name="name">The name of the sub-module (displayed in progress UI).</param>
    /// <param name="action">The delegate that implements the sub-module's logic.</param>
    /// <returns>The result of the sub-module execution.</returns>
    Task<TResult> ExecuteSubModuleAsync<TResult>(IModule parentModule, string name, Func<Task<TResult>> action);
}
