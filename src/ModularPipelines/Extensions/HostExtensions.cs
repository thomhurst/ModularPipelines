using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine.Executors;
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
            if (!IsRunningFromNUnit)
            {
                host.Dispose();
            }
        }
    }

    private static readonly bool IsRunningFromNUnit = 
        AppDomain.CurrentDomain.GetAssemblies().Any(
            a => a.FullName?.ToLowerInvariant().StartsWith("nunit.framework") == true);
}