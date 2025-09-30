using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Distributed.Modules;

/// <summary>
/// Simulates fetching data from an external source.
/// This module has no dependencies and can run immediately.
/// </summary>
public class FetchDataModule : Module<string?>
{
    protected override async Task<string> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("📥 Fetching data from external source...");

        // Simulate network delay
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);

        var data = $"Data fetched at {DateTime.UtcNow:HH:mm:ss}";

        context.Logger.LogInformation("✓ Data fetched successfully: {Data}", data);

        return data;
    }
}
