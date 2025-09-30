using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Distributed.Modules;

/// <summary>
/// Processes the data fetched by FetchDataModule.
/// This module depends on FetchDataModule and will run after it completes.
/// </summary>
[DependsOn<FetchDataModule>]
public class ProcessDataModule : Module<string?>
{
    protected override async Task<string> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        // Get the result from the dependency
        var fetchedData = await GetModule<FetchDataModule>();
        var inputData = fetchedData.Value ?? "No data";

        context.Logger.LogInformation("⚙️ Processing data: {Data}", inputData);

        // Simulate processing time
        await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);

        var processedData = $"Processed: {inputData} (completed at {DateTime.UtcNow:HH:mm:ss})";

        context.Logger.LogInformation("✓ Data processed successfully");

        return processedData;
    }
}
