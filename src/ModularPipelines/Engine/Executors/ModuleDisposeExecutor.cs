using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Options;

namespace ModularPipelines.Engine.Executors;

[StackTraceHidden]
internal class ModuleDisposeExecutor : IModuleDisposeExecutor
{
    private readonly IModuleDisposer _moduleDisposer;
    private readonly IOptions<PipelineOptions> _options;
    private readonly IModuleRetriever _moduleRetriever;

    public ModuleDisposeExecutor(IModuleDisposer moduleDisposer, IOptions<PipelineOptions> options, IModuleRetriever moduleRetriever)
    {
        _moduleDisposer = moduleDisposer;
        _options = options;
        _moduleRetriever = moduleRetriever;
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
                await _moduleDisposer.DisposeAsync(module);
            }
            catch (Exception e)
            {
                module.Context?.Logger.LogError(e, "Error disposing module");
            }
        }
    }
}