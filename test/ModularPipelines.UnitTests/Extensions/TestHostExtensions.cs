using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Host;

namespace ModularPipelines.UnitTests.Extensions;

public static class TestHostExtensions
{
    public static async Task<IServiceProvider> ExecuteTest(this IPipelineHost host)
    {
        try
        {
            await host.Services.GetRequiredService<IExecutionOrchestrator>().ExecuteAsync();
            return host.Services;
        }
        catch
        {
            return host.Services;
        }
    }
}