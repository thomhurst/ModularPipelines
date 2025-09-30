using Microsoft.Extensions.Logging;
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
            .Where(x => x.ServiceType == typeof(ModuleBase))
            .Select(x => x.ImplementationType);

        var allDetectedModules = _assemblyLoadedTypesProvider.GetLoadedTypesAssignableTo(typeof(ModuleBase));

        var unregisteredModules = allDetectedModules
            .Except(registeredServices)
            .ToList();

        if (unregisteredModules.Count == 0)
        {
            return;
        }

        _logger.LogWarning("[yellow]⚠[/] [bold]Unregistered Modules:[/]\n{Modules}",
            string.Join("\n  • ", unregisteredModules.Select(m => m?.Name)));
    }
}