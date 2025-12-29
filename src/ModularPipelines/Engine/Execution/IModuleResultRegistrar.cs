using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for registering module execution results.
/// Handles successful results, failed results, and terminated results for cancelled modules.
/// </summary>
internal interface IModuleResultRegistrar
{
    /// <summary>
    /// Registers a terminated result for a module that was cancelled before completion.
    /// </summary>
    /// <param name="module">The module instance.</param>
    /// <param name="moduleType">The type of the module.</param>
    /// <param name="exception">The exception that caused termination.</param>
    void RegisterTerminatedResult(IModule module, Type moduleType, Exception exception);

    /// <summary>
    /// Registers terminated results for all modules that were cancelled before they started.
    /// </summary>
    /// <param name="modules">The list of all modules.</param>
    /// <param name="exception">The exception that caused pipeline termination.</param>
    void RegisterTerminatedResultsForCancelledModules(IReadOnlyList<IModule> modules, Exception exception);
}
