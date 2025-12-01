using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class UnusedModuleDetector : IUnusedModuleDetector
{
    private readonly IAssemblyLoadedTypesProvider _assemblyLoadedTypesProvider;
    private readonly IPipelineServiceContainerWrapper _serviceContainerWrapper;
    private readonly ILogger<UnusedModuleDetector> _logger;

    public UnusedModuleDetector(IAssemblyLoadedTypesProvider assemblyLoadedTypesProvider,
        IPipelineServiceContainerWrapper serviceContainerWrapper,
        ILogger<UnusedModuleDetector> logger)
    {
        _assemblyLoadedTypesProvider = assemblyLoadedTypesProvider;
        _serviceContainerWrapper = serviceContainerWrapper;
        _logger = logger;
    }

    public void Log()
    {
        var registeredServices = _serviceContainerWrapper.ServiceCollection
            .Where(x => x.ServiceType == typeof(IModule))
            .Select(x => x.ImplementationType)
            .ToHashSet();

        var allDetectedModules = _assemblyLoadedTypesProvider.GetLoadedTypesAssignableTo(typeof(IModule));

        var unregisteredModules = allDetectedModules
            .Except(registeredServices)
            .ToList();

        var registeredModuleTypes = _serviceContainerWrapper.ServiceCollection
            .Where(x => x.ServiceType == typeof(IModule) && x.ImplementationType != null)
            .Select(x => x.ImplementationType!)
            .ToList();

        var unregisteredDependencies = registeredModuleTypes
            .SelectMany(moduleType => moduleType.GetCustomAttributes(typeof(DependsOnAttribute), inherit: true)
                .Cast<DependsOnAttribute>())
            .Where(attr => !attr.IgnoreIfNotRegistered)
            .Select(attr => attr.Type)
            .Distinct()
            .Where(depType => !registeredServices.Contains(depType))
            .ToList();

        if (unregisteredModules.Count == 0 && unregisteredDependencies.Count == 0)
        {
            return;
        }

        var allUnregistered = unregisteredModules.Concat(unregisteredDependencies).Distinct().ToList();

        _logger.LogWarning("⚠ Unregistered Modules:\n{Modules}",
            string.Join("\n", allUnregistered.Select(m => $"  • {m?.Name}")));
    }
}