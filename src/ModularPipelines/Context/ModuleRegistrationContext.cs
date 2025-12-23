// src/ModularPipelines/Context/ModuleRegistrationContext.cs
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Modules;

namespace ModularPipelines.Context;

/// <summary>
/// Implementation of <see cref="IModuleRegistrationContext"/>.
/// </summary>
internal class ModuleRegistrationContext : IModuleRegistrationContext
{
    private readonly IModuleDependencyRegistry _dependencyRegistry;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly IReadOnlyList<Type> _registeredModuleTypes;

    public ModuleRegistrationContext(
        Type moduleType,
        IReadOnlyList<Attribute> moduleAttributes,
        IConfiguration configuration,
        IHostEnvironment environment,
        IReadOnlyList<Type> registeredModuleTypes,
        IServiceCollection services,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry)
    {
        ModuleType = moduleType;
        ModuleAttributes = moduleAttributes;
        Configuration = configuration;
        Environment = environment;
        _registeredModuleTypes = registeredModuleTypes;
        Services = services;
        _dependencyRegistry = dependencyRegistry;
        _metadataRegistry = metadataRegistry;
    }

    /// <summary>
    /// Initialises a new instance without IServiceCollection (for post-container-build scenarios).
    /// </summary>
    public ModuleRegistrationContext(
        Type moduleType,
        IReadOnlyList<Attribute> moduleAttributes,
        IConfiguration configuration,
        IHostEnvironment environment,
        IReadOnlyList<Type> registeredModuleTypes,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry)
    {
        ModuleType = moduleType;
        ModuleAttributes = moduleAttributes;
        Configuration = configuration;
        Environment = environment;
        _registeredModuleTypes = registeredModuleTypes;
        Services = null!;
        _dependencyRegistry = dependencyRegistry;
        _metadataRegistry = metadataRegistry;
    }

    public Type ModuleType { get; }

    public IReadOnlyList<Attribute> ModuleAttributes { get; }

    public IConfiguration Configuration { get; }

    public IHostEnvironment Environment { get; }

    public IReadOnlyList<Type> RegisteredModuleTypes => _registeredModuleTypes;

    public IServiceCollection Services { get; }

    public bool IsModuleRegistered<TModule>() where TModule : IModule
        => IsModuleRegistered(typeof(TModule));

    public bool IsModuleRegistered(Type moduleType)
        => _registeredModuleTypes.Contains(moduleType);

    public IEnumerable<Type> GetModulesAssignableTo<TBase>() where TBase : IModule
        => _registeredModuleTypes.Where(t => typeof(TBase).IsAssignableFrom(t) && t != ModuleType);

    public IEnumerable<Type> GetModulesWithAttribute<TAttribute>() where TAttribute : Attribute
        => _registeredModuleTypes.Where(t => t.GetCustomAttribute<TAttribute>() != null);

    public void AddDependency<TModule>() where TModule : IModule
        => AddDependency(typeof(TModule));

    public void AddDependency(Type moduleType)
        => _dependencyRegistry.AddDynamicDependency(ModuleType, moduleType);

    public void AddDependencyOnAll<TBase>() where TBase : IModule
    {
        foreach (var moduleType in GetModulesAssignableTo<TBase>())
        {
            AddDependency(moduleType);
        }
    }

    public void AddDependencyOnAll(Func<Type, bool> predicate)
    {
        foreach (var moduleType in _registeredModuleTypes.Where(t => t != ModuleType && predicate(t)))
        {
            AddDependency(moduleType);
        }
    }

    public void RemoveDependency<TModule>() where TModule : IModule
        => _dependencyRegistry.RemoveDependency(ModuleType, typeof(TModule));

    public void SetMetadata(string key, object value)
        => _metadataRegistry.SetMetadata(ModuleType, key, value);

    public T? GetMetadata<T>(string key)
        => _metadataRegistry.GetMetadata<T>(ModuleType, key);
}
