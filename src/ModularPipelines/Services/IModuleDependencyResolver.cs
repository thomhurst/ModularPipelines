using ModularPipelines.Modules;

namespace ModularPipelines.Services;

/// <summary>
/// Service responsible for resolving module dependencies.
/// This replaces the GetModule methods previously embedded in ModuleBase.
/// </summary>
public interface IModuleDependencyResolver
{
    /// <summary>
    /// Resolves a required module by type.
    /// </summary>
    /// <typeparam name="TModule">The type of module to resolve.</typeparam>
    /// <returns>The resolved module instance.</returns>
    /// <exception cref="ModularPipelines.Exceptions.ModuleNotRegisteredException">
    /// Thrown if the module is not registered in the pipeline.
    /// </exception>
    Task<TModule> GetModuleAsync<TModule>() where TModule : IModule;

    /// <summary>
    /// Resolves an optional module by type.
    /// </summary>
    /// <typeparam name="TModule">The type of module to resolve.</typeparam>
    /// <returns>The resolved module instance, or null if not registered.</returns>
    Task<TModule?> GetModuleIfRegisteredAsync<TModule>() where TModule : IModule;

    /// <summary>
    /// Gets the module dependencies for a given module type.
    /// This reads the [DependsOn] attributes and returns the dependency types.
    /// </summary>
    /// <param name="moduleType">The module type to analyze.</param>
    /// <returns>A list of dependency types and whether they're optional.</returns>
    IReadOnlyList<(Type DependencyType, bool IgnoreIfNotRegistered)> GetDependencies(Type moduleType);
}
