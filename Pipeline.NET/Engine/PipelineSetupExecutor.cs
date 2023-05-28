using Pipeline.NET.Context;
using Pipeline.NET.Interfaces;

namespace Pipeline.NET.Engine;

public class PipelineSetupExecutor : IPipelineSetupExecutor
{
    private readonly IModuleContext _moduleContext;
    private readonly IEnumerable<IPipelineGlobalSetup> _globalSetups;

    public PipelineSetupExecutor(IModuleContext moduleContext, IEnumerable<IPipelineGlobalSetup> globalSetups)
    {
        _moduleContext = moduleContext;
        _globalSetups = globalSetups;
    }
    
    public Task OnStartAsync()
    {
        return Task.WhenAll(_globalSetups.Select(x => x.OnStartAsync(_moduleContext)));
    }

    public Task OnEndAsync()
    {
        return Task.WhenAll(_globalSetups.Select(x => x.OnEndAsync(_moduleContext)));
    }
}