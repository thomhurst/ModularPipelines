using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;

namespace ModularPipelines.Events;

/// <summary>
/// Context provided to <see cref="IModuleRegistrationHandler.OnRegistrationAsync"/>.
/// Provides pipeline configuration and dynamic dependency operations.
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
    /// Checks whether a module of the specified type is registered.
    /// </summary>
    /// <typeparam name="TModule">The module type.</typeparam>
    /// <returns><see langword="true"/> when the module is registered; otherwise, <see langword="false"/>.</returns>
    bool IsModuleRegistered<TModule>()
        where TModule : IModule;

    /// <summary>
    /// Checks whether a module of the specified type is registered.
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    /// <returns><see langword="true"/> when the module is registered; otherwise, <see langword="false"/>.</returns>
    bool IsModuleRegistered(Type moduleType);

    /// <summary>
    /// Gets all registered module types assignable to the specified base type.
    /// </summary>
    /// <typeparam name="TBase">The base module type.</typeparam>
    /// <returns>The matching registered module types.</returns>
    IEnumerable<Type> GetModulesAssignableTo<TBase>()
        where TBase : IModule;

    /// <summary>
    /// Gets all registered module types with the specified attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The attribute type.</typeparam>
    /// <returns>The matching registered module types.</returns>
    IEnumerable<Type> GetModulesWithAttribute<TAttribute>()
        where TAttribute : Attribute;

    /// <summary>
    /// Adds a dependency on the specified module.
    /// </summary>
    /// <typeparam name="TModule">The dependency module type.</typeparam>
    void AddDependency<TModule>()
        where TModule : IModule;

    /// <summary>
    /// Adds a dependency on the specified module.
    /// </summary>
    /// <param name="moduleType">The dependency module type.</param>
    void AddDependency(Type moduleType);

    /// <summary>
    /// Adds dependencies on all modules assignable to the specified base type.
    /// </summary>
    /// <typeparam name="TBase">The base module type.</typeparam>
    void AddDependencyOnAll<TBase>()
        where TBase : IModule;

    /// <summary>
    /// Adds dependencies on all modules matching the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate used to select module types.</param>
    void AddDependencyOnAll(Func<Type, bool> predicate);

    /// <summary>
    /// Removes a dynamic dependency on the specified module.
    /// </summary>
    /// <typeparam name="TModule">The dependency module type.</typeparam>
    /// <remarks>
    /// Only dependencies added during registration are removed. Declared dependencies are unchanged.
    /// Removing an absent dependency has no effect.
    /// </remarks>
    void RemoveDependency<TModule>()
        where TModule : IModule;

    /// <summary>
    /// Removes a dynamic dependency on the specified module.
    /// </summary>
    /// <param name="moduleType">The dependency type, which must implement <see cref="IModule"/>.</param>
    /// <remarks>
    /// Only dependencies added during registration are removed. Declared dependencies are unchanged.
    /// Removing an absent dependency has no effect, even when the module is not registered.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="moduleType"/> is null.</exception>
    /// <exception cref="InvalidModuleTypeException"><paramref name="moduleType"/> does not implement <see cref="IModule"/>.</exception>
    void RemoveDependency(Type moduleType);

    /// <summary>
    /// Sets metadata that can be retrieved during module execution.
    /// </summary>
    /// <param name="key">The metadata key.</param>
    /// <param name="value">The metadata value.</param>
    void SetMetadata(string key, object value);

    /// <summary>
    /// Gets metadata set during registration.
    /// </summary>
    /// <typeparam name="T">The metadata value type.</typeparam>
    /// <param name="key">The metadata key.</param>
    /// <returns>The metadata value, or the default value when the key is absent.</returns>
    T? GetMetadata<T>(string key);
}
