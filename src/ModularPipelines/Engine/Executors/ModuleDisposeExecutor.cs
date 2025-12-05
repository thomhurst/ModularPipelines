using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine.Executors;

[StackTraceHidden]
internal class ModuleDisposeExecutor : IModuleDisposeExecutor
{
    private readonly IModuleDisposer _moduleDisposer;
    private readonly IOptions<PipelineOptions> _options;
    private readonly IModuleRetriever _moduleRetriever;
    private readonly ILogger<ModuleDisposeExecutor> _logger;

    public ModuleDisposeExecutor(
        IModuleDisposer moduleDisposer,
        IOptions<PipelineOptions> options,
        IModuleRetriever moduleRetriever,
        ILogger<ModuleDisposeExecutor> logger)
    {
        _moduleDisposer = moduleDisposer;
        _options = options;
        _moduleRetriever = moduleRetriever;
        _logger = logger;
    }

    public async ValueTask DisposeAsync()
    {
        if (!_options.Value.ShowProgressInConsole)
        {
            // If not an interactive console, we'll dispose each module as it finishes, to print its output
            // Otherwise we'll do it here, so we don't mess up the Progress printer
            return;
        }

        var modules = await _moduleRetriever.GetOrganizedModules();

        foreach (var module in modules.AllModules)
        {
            try
            {
                await _moduleDisposer.DisposeAsync((IModule) module);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error disposing module {ModuleType}", module.GetType().Name);
            }
        }
    }
}