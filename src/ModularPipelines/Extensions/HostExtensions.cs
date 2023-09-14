using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Helpers;
using ModularPipelines.Host;
using ModularPipelines.Models;

namespace ModularPipelines.Extensions;

internal static class HostExtensions
{
    public static async Task<PipelineSummary> ExecutePipelineAsync(this IPipelineHost host)
    {
        try
        {
            return await host.Services.GetRequiredService<IExecutionOrchestrator>().ExecuteAsync();
        }
        finally
        {
            if (!TestDetector.IsRunningFromNUnit)
            {
                await Disposer.DisposeObjectAsync(host);
            }
        }
    }
}