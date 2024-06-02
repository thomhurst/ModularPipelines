using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;

namespace ModularPipelines.Logging;

internal class AfterPipelineLogger : IPipelineGlobalHooks
{
    private readonly List<string> _values = [];
    
    public void LogOnPipelineEnd(string value)
    {
        _values.Add(value);
    }
    
    public Task OnStartAsync(IPipelineHookContext pipelineContext)
    {
        return Task.CompletedTask;
    }

    public Task OnEndAsync(IPipelineHookContext pipelineContext, PipelineSummary pipelineSummary)
    {
        foreach (var value in _values)
        {
            pipelineContext.Logger.LogInformation("{Value}", value);
        }
        
        return Task.CompletedTask;
    }
}