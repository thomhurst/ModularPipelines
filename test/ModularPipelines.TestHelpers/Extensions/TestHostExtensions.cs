using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Host;

namespace ModularPipelines.TestHelpers.Extensions;

public static class TestHostExtensions
{
    /// <summary>
    /// Executes the pipeline with default timeout protection.
    /// </summary>
    public static Task<IServiceProvider> ExecuteTest(this IPipelineHost host)
        => ExecuteTest(host, TestHostSettings.DefaultTestTimeout);

    /// <summary>
    /// Executes the pipeline with the specified timeout.
    /// </summary>
    public static async Task<IServiceProvider> ExecuteTest(this IPipelineHost host, TimeSpan timeout)
    {
        using var cts = timeout == Timeout.InfiniteTimeSpan
            ? new CancellationTokenSource()
            : new CancellationTokenSource(timeout);

        try
        {
            await host.Services.GetRequiredService<IExecutionOrchestrator>().ExecuteAsync(cts.Token);
            return host.Services;
        }
        catch
        {
            return host.Services;
        }
    }
}