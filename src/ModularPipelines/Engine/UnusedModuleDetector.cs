using Microsoft.Extensions.Logging;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Extensions;
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
        var debugEnabled = _logger.IsEnabled(LogLevel.Debug);
        var warningEnabled = _logger.IsEnabled(LogLevel.Warning);
        if (!debugEnabled && !warningEnabled)
        {
            return;
        }

        // Use the centralized helper that works with factory-based registrations
        var registeredServices = ServiceCollectionExtensions.GetRegisteredModuleTypes(_serviceContainerWrapper.ServiceCollection);

        var unregisteredDependencies = registeredServices
            .SelectMany(ModuleDependencyResolver.GetDependencies)
            .Where(static dependency => !dependency.Optional)
            .Select(static dependency => dependency.DependencyType)
            .Distinct()
            .Where(depType => !registeredServices.Contains(depType))
            .ToList();

        if (unregisteredDependencies.Count > 0)
        {
            if (warningEnabled)
            {
                _logger.LogWarning("⚠ Missing Required Module Dependencies:\n{Modules}",
                    string.Join("\n", unregisteredDependencies.Select(static module => $"  • {module.Name}")));
            }
            else
            {
                _logger.LogDebug(
                    "Missing required module dependencies ({Count}): {Modules}",
                    unregisteredDependencies.Count,
                    string.Join(", ", unregisteredDependencies.Select(static module => module.Name)));
            }
        }

        if (!debugEnabled)
        {
            return;
        }

        var loadedButUnusedModules = _assemblyLoadedTypesProvider
            .GetLoadedTypesAssignableTo(typeof(IModule))
            .Except(registeredServices)
            .Except(unregisteredDependencies)
            .Select(static module => module.Name)
            .Order(StringComparer.Ordinal)
            .ToList();

        if (loadedButUnusedModules.Count > 0)
        {
            _logger.LogDebug(
                "Loaded module types not registered ({Count}): {Modules}",
                loadedButUnusedModules.Count,
                string.Join(", ", loadedButUnusedModules));
        }
    }
}
