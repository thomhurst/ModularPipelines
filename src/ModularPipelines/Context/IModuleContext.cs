using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Context;

/// <summary>
/// Context provided to modules during execution, extending pipeline context with module-specific capabilities.
/// </summary>
public interface IModuleContext : IPipelineContext
{
    /// <summary>
    /// Gets a module by type and returns its result when awaited.
    /// </summary>
    /// <typeparam name="TModule">The type of module to retrieve.</typeparam>
    /// <returns>The module result when awaited.</returns>
    /// <exception cref="Exceptions.ModuleNotRegisteredException">Thrown if the module is not registered.</exception>
    /// <exception cref="Exceptions.ModuleReferencingSelfException">Thrown if a module tries to get itself.</exception>
    ModuleResult<TResult> GetModule<TModule, TResult>()
        where TModule : Module<TResult>;

    /// <summary>
    /// Gets a module by type if it is registered, or null otherwise.
    /// </summary>
    /// <typeparam name="TModule">The type of module to retrieve.</typeparam>
    /// <returns>The module result when awaited, or null if not registered.</returns>
    ModuleResult<TResult>? GetModuleIfRegistered<TModule, TResult>()
        where TModule : Module<TResult>;

    /// <summary>
    /// Tracks a sub-operation within the current module for progress display.
    /// </summary>
    /// <typeparam name="T">The result type of the sub-operation.</typeparam>
    /// <param name="name">A descriptive name for the sub-operation.</param>
    /// <param name="action">The async operation to execute.</param>
    /// <returns>The result of the sub-operation.</returns>
    Task<T> SubModule<T>(string name, Func<Task<T>> action);

    /// <summary>
    /// Tracks a sub-operation within the current module for progress display.
    /// </summary>
    /// <param name="name">A descriptive name for the sub-operation.</param>
    /// <param name="action">The async operation to execute.</param>
    /// <returns>A task representing the sub-operation.</returns>
    Task SubModule(string name, Func<Task> action);
}
