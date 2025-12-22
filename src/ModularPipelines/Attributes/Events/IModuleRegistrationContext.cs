using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Modules;

namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Context provided to <see cref="IModuleRegistrationEventReceiver.OnRegistrationAsync"/>.
/// Provides access to pipeline configuration and enables dynamic dependency manipulation.
/// </summary>
public interface IModuleRegistrationContext
{
    /// <summary>
    /// Gets the type of the module being registered.
    /// </summary>
    Type ModuleType { get; }

    /// <summary>
    /// Gets the attributes declared on the module.
    /// </summary>
    IReadOnlyList<Attribute> ModuleAttributes { get; }

    /// <summary>
    /// Gets the application configuration.
    /// </summary>
    IConfiguration Configuration { get; }

    /// <summary>
    /// Gets the host environment information.
    /// </summary>
    IHostEnvironment Environment { get; }

    /// <summary>
    /// Gets the types of all registered modules.
    /// </summary>
    IReadOnlyList<Type> RegisteredModuleTypes { get; }

    /// <summary>
    /// Checks if a specific module type is registered.
    /// </summary>
    bool IsModuleRegistered<TModule>() where TModule : IModule;

    /// <summary>
    /// Checks if a specific module type is registered.
    /// </summary>
    bool IsModuleRegistered(Type moduleType);

    /// <summary>
    /// Gets all registered module types that are assignable to the specified base type.
    /// </summary>
    IEnumerable<Type> GetModulesAssignableTo<TBase>() where TBase : IModule;

    /// <summary>
    /// Gets all registered module types that have the specified attribute.
    /// </summary>
    IEnumerable<Type> GetModulesWithAttribute<TAttribute>() where TAttribute : Attribute;

    /// <summary>
    /// Adds a dependency on another module.
    /// </summary>
    void AddDependency<TModule>() where TModule : IModule;

    /// <summary>
    /// Adds a dependency on another module.
    /// </summary>
    void AddDependency(Type moduleType);

    /// <summary>
    /// Adds dependencies on all modules assignable to the specified base type.
    /// </summary>
    void AddDependencyOnAll<TBase>() where TBase : IModule;

    /// <summary>
    /// Adds dependencies on all modules matching the specified predicate.
    /// </summary>
    void AddDependencyOnAll(Func<Type, bool> predicate);

    /// <summary>
    /// Removes a dependency on another module.
    /// </summary>
    void RemoveDependency<TModule>() where TModule : IModule;

    /// <summary>
    /// Gets the service collection for registering additional services.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Sets metadata that can be retrieved during module execution.
    /// </summary>
    void SetMetadata(string key, object value);

    /// <summary>
    /// Gets metadata that was set during registration.
    /// </summary>
    T? GetMetadata<T>(string key);
}
