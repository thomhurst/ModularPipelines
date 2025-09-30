using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Distributed.Modules;

/// <summary>
/// Publishes the final results.
/// This module depends on both ProcessDataModule and ValidateEnvironmentModule,
/// demonstrating how the scheduler handles complex dependency graphs.
/// </summary>
[DependsOn<ProcessDataModule>]
[DependsOn<ValidateEnvironmentModule>]
public class PublishResultsModule : Module<string?>
{
    protected override async Task<string> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        // Get results from dependencies
        var processedData = await GetModule<ProcessDataModule>();
        var validationResult = await GetModule<ValidateEnvironmentModule>();

        context.Logger.LogInformation(
            "📤 Publishing results (Data: {Data}, Environment Valid: {IsValid})",
            processedData.Value,
            validationResult.Value);

        // Simulate publishing
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);

        var result = $"Published at {DateTime.UtcNow:HH:mm:ss}";

        context.Logger.LogInformation("✓ Results published successfully");

        return result;
    }
}
