using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Services;

/// <summary>
/// Service for resolving module dependencies from the DI container.
/// Replaces the GetModule methods previously embedded in ModuleBase.
/// </summary>
public class ModuleDependencyResolver : IModuleDependencyResolver
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Type, IModule> _moduleCache = new();

    public ModuleDependencyResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TModule> GetModuleAsync<TModule>() where TModule : IModule
    {
        var module = await GetModuleIfRegisteredAsync<TModule>();

        if (module is null)
        {
            throw new ModuleNotRegisteredException(
                $"The module '{typeof(TModule).Name}' has not been registered in the pipeline.\n\n" +
                $"Suggestions:\n" +
                $"  1. Add '.AddModule<{typeof(TModule).Name}>()' to your pipeline configuration\n" +
                $"  2. Use 'GetModuleIfRegisteredAsync<{typeof(TModule).Name}>()' if this module might not be present\n" +
                $"  3. Ensure '{typeof(TModule).Name}' is registered before modules that depend on it",
                null);
        }

        return module;
    }

    public Task<TModule?> GetModuleIfRegisteredAsync<TModule>() where TModule : IModule
    {
        var moduleType = typeof(TModule);

        // Check cache first
        if (_moduleCache.TryGetValue(moduleType, out var cachedModule))
        {
            return Task.FromResult((TModule?)cachedModule);
        }

        // Resolve from DI container
        var module = _serviceProvider.GetService<TModule>();

        // Cache if found
        if (module != null)
        {
            _moduleCache[moduleType] = module;
        }

        return Task.FromResult(module);
    }

    public IReadOnlyList<(Type DependencyType, bool IgnoreIfNotRegistered)> GetDependencies(Type moduleType)
    {
        var dependencies = new List<(Type, bool)>();

        // Get [DependsOn<T>] attributes
        foreach (var attr in moduleType.GetCustomAttributesIncludingBaseInterfaces<DependsOnAttribute>())
        {
            dependencies.Add((attr.Type, attr.IgnoreIfNotRegistered));
        }

        // Get [DependsOnAllModulesInheritingFrom<T>] attributes
        foreach (var attr in moduleType.GetCustomAttributesIncludingBaseInterfaces<DependsOnAllModulesInheritingFromAttribute>())
        {
            // Resolve all modules that inherit from the specified type
            var modules = _serviceProvider.GetServices<IModule>()
                .Where(m => m.ModuleType.IsOrInheritsFrom(attr.Type));

            foreach (var module in modules)
            {
                dependencies.Add((module.ModuleType, false));
            }
        }

        return dependencies;
    }
}
