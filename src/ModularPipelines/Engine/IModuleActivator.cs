using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Activates module instances with proper AsyncLocal context for logging.
/// </summary>
/// <remarks>
/// This interface exists to ensure that when modules are constructed via DI,
/// the AsyncLocal context for logging is properly set before the constructor runs.
/// This allows module constructors to log with proper module context.
/// </remarks>
internal interface IModuleActivator
{
    /// <summary>
    /// Creates a module instance with AsyncLocal context set for the module type.
    /// </summary>
    /// <param name="moduleType">The type of module to create.</param>
    /// <param name="serviceProvider">The service provider for resolving dependencies.</param>
    /// <returns>The created module instance.</returns>
    IModule CreateModule(Type moduleType, IServiceProvider serviceProvider);
}
