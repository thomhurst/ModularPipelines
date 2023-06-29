using Microsoft.Extensions.Logging;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleResultPrinter : IModuleResultPrinter
{
    private readonly IEnumerable<ModuleBase> _modules;
    private readonly ILogger<ModuleResultPrinter> _logger;

    public ModuleResultPrinter(IEnumerable<ModuleBase> modules, ILogger<ModuleResultPrinter> logger)
    {
        _modules = modules;
        _logger = logger;
    }
    
    public void PrintModuleResults()
    {
        foreach (var moduleBase in _modules)
        {
            _logger.LogInformation("{Module} Result: {Result}", moduleBase.GetType().Name, moduleBase.Status.ToString());
        }
    }
}