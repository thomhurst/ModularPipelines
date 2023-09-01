using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Modules;

namespace ModularPipelines.Extensions;

internal static class HostExtensions
{
    public static async Task<IReadOnlyList<ModuleBase>> ExecuteAsync(this IHost host)
    {
        await host.Services.GetRequiredService<IServiceProviderInitializer>().InitializeAsync();

        try
        {
            return await host.Services.GetRequiredService<IExecutionOrchestrator>().ExecuteAsync();
        }
        finally
        {
            await ((ServiceProvider) host.Services).DisposeAsync();
        }
    }
}