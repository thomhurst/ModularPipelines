using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine.Executors;

namespace ModularPipelines.TestHelpers.Extensions;

public static class TestHostExtensions
{
    /// <summary>
    /// Executes the pipeline with default timeout protection.
    /// </summary>
    public static Task<IServiceProvider> ExecuteTest(this IPipeline pipeline)
        => ExecuteTest(pipeline, TestHostSettings.DefaultTestTimeout);

    /// <summary>
    /// Executes the pipeline with the specified timeout.
    /// </summary>
    public static async Task<IServiceProvider> ExecuteTest(this IPipeline pipeline, TimeSpan timeout)
    {
        using var cts = timeout == Timeout.InfiniteTimeSpan
            ? new CancellationTokenSource()
            : new CancellationTokenSource(timeout);

        try
        {
            await pipeline.Services.GetRequiredService<IExecutionOrchestrator>().ExecuteAsync(cts.Token);
            return pipeline.Services;
        }
        catch
        {
            return pipeline.Services;
        }
    }
}
