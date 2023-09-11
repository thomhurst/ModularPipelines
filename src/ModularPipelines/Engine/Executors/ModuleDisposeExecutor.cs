using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Spectre.Console;

namespace ModularPipelines.Engine.Executors;

internal class ModuleDisposeExecutor : IModuleDisposeExecutor
{
    private readonly IModuleDisposer _moduleDisposer;
    private readonly IOptions<PipelineOptions> _options;

    public ModuleDisposeExecutor(IModuleDisposer moduleDisposer, IOptions<PipelineOptions> options)
    {
        _moduleDisposer = moduleDisposer;
        _options = options;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task ExecuteAndDispose(IEnumerable<ModuleBase> modules, Func<Task> executeDelegate)
    {
        try
        {
            await executeDelegate();
        }
        finally
        {
            await Dispose(modules);
        }
    }
    
    private async Task Dispose(IEnumerable<ModuleBase> modulesToProcess)
    {
        if (!AnsiConsole.Profile.Capabilities.Interactive || !_options.Value.ShowProgressInConsole)
        {
            // If not an interactive console, we'll dispose each module as it finishes, to print its output
            // Otherwise we'll do it here, so we don't miss up the Progress printer
            return;
        }
        
        foreach (var module in modulesToProcess)
        {
            await _moduleDisposer.DisposeAsync(module);
        }
    }
}