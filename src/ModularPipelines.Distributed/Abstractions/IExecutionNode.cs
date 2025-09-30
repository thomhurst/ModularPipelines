using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Abstractions;

/// <summary>
/// Represents an abstraction for executing modules either locally or remotely.
/// </summary>
public interface IExecutionNode
{
    /// <summary>
    /// Gets the unique identifier for this execution node.
    /// </summary>
    string NodeId { get; }

    /// <summary>
    /// Gets a value indicating whether this node can execute the specified module.
    /// </summary>
    /// <param name="module">The module to check.</param>
    /// <returns>True if the node can execute the module; otherwise, false.</returns>
    bool CanExecute(ModuleBase module);

    /// <summary>
    /// Executes a module on this node.
    /// </summary>
    /// <param name="module">The module to execute.</param>
    /// <param name="dependencyResults">Results from dependent modules.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The module result.</returns>
    Task<IModuleResult> ExecuteAsync(
        ModuleBase module,
        IReadOnlyDictionary<Type, IModuleResult> dependencyResults,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current load on this execution node.
    /// </summary>
    /// <returns>The number of modules currently executing.</returns>
    int GetCurrentLoad();
}
