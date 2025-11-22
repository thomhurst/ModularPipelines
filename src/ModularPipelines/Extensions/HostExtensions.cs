using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Host;
using ModularPipelines.Models;

namespace ModularPipelines.Extensions;

internal static class HostExtensions
{
    public static async Task<PipelineSummary> ExecutePipelineAsync(this IPipelineHost host)
    {
        return await host.Services.GetRequiredService<IExecutionOrchestrator>().ExecuteAsync();
    }
}
