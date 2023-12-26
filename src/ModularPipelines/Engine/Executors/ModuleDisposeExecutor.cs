using System.Diagnostics;
using Microsoft.Extensions.Options;
using ModularPipelines.Options;
using Spectre.Console;

namespace ModularPipelines.Engine.Executors;

[StackTraceHidden]
internal class ModuleDisposeExecutor : IModuleDisposeExecutor
{
    private readonly IModuleDisposer _moduleDisposer;
    private readonly IOptions<PipelineOptions> _options;
    private readonly IPipelineInitializer _pipelineInitializer;

    public ModuleDisposeExecutor(IModuleDisposer moduleDisposer, IOptions<PipelineOptions> options, IPipelineInitializer pipelineInitializer)
    {
        _moduleDisposer = moduleDisposer;
        _options = options;
        _pipelineInitializer = pipelineInitializer;
    }

    public async ValueTask DisposeAsync()
    {
        if (!AnsiConsole.Profile.Capabilities.Interactive || !_options.Value.ShowProgressInConsole)
        {
            // If not an interactive console, we'll dispose each module as it finishes, to print its output
            // Otherwise we'll do it here, so we don't miss up the Progress printer
            return;
        }

        var modules = await _pipelineInitializer.Initialize();
        
        foreach (var module in modules.AllModules)
        {
            await _moduleDisposer.DisposeAsync(module);
        }    
    }
}