namespace ModularPipelines.Modules.Behaviors;

/// <summary>
/// Implement this interface to enable sub-module support for a module.
/// Sub-modules provide progress tracking for long-running operations within a module.
/// </summary>
public interface IModuleSubModules
{
    /// <summary>
    /// Executes a sub-module with progress tracking.
    /// </summary>
    /// <typeparam name="TResult">The type of result the sub-module produces.</typeparam>
    /// <param name="name">The name of the sub-module (displayed in progress UI).</param>
    /// <param name="action">The delegate that implements the sub-module's logic.</param>
    /// <returns>The result of the sub-module execution.</returns>
    Task<TResult> SubModuleAsync<TResult>(string name, Func<Task<TResult>> action);
}
