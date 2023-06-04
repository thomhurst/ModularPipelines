using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal class ModuleRetriever : IModuleRetriever
{
    private readonly IModuleIgnoreHandler _moduleIgnoreHandler;
    private readonly List<ModuleBase> _modules;

    public ModuleRetriever(
        IModuleIgnoreHandler moduleIgnoreHandler,
        IEnumerable<ModuleBase> modules
    )
    {
        _moduleIgnoreHandler = moduleIgnoreHandler;
        _modules = modules.ToList();
    }

    public OrganizedModules GetOrganizedModules()
    {
        if (_modules.Count == 0)
        {
            throw new PipelineException("No modules have been registered");
        }
        
        var modulesToIgnore = _modules
            .Where(m => _moduleIgnoreHandler.ShouldIgnore(m))
            .ToList();

        var modulesToProcess = _modules
            .Except(modulesToIgnore)
            .ToList();

        return new OrganizedModules(
            RunnableModules: modulesToProcess,
            IgnoredModules: modulesToIgnore
        );
    }
}