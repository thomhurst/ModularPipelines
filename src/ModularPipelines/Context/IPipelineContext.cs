using ModularPipelines.Modules;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to pipeline context including modules and configuration.
/// </summary>
public interface IPipelineContext : IPipelineHookContext
{
    internal TModule? GetModule<TModule>()
        where TModule : ModuleBase;

    internal ModuleBase? GetModule(Type type);

    /// <summary>
    /// Gets a required module dependency asynchronously.
    /// </summary>
    /// <typeparam name="TModule">The type of module to retrieve.</typeparam>
    /// <returns>The module instance.</returns>
    /// <exception cref="ModularPipelines.Exceptions.ModuleNotRegisteredException">
    /// Thrown if the module is not registered in the pipeline.
    /// </exception>
    Task<TModule> GetModuleAsync<TModule>() where TModule : IModule;

    /// <summary>
    /// Gets an optional module dependency asynchronously.
    /// </summary>
    /// <typeparam name="TModule">The type of module to retrieve.</typeparam>
    /// <returns>The module instance, or null if not registered.</returns>
    Task<TModule?> GetModuleIfRegisteredAsync<TModule>() where TModule : IModule;
}