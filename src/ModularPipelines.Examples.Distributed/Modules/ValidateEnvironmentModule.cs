using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Distributed.Modules;

/// <summary>
/// Validates the environment is properly configured.
/// This module has no dependencies and can run in parallel with FetchDataModule.
/// </summary>
public class ValidateEnvironmentModule : Module<bool>
{
    protected override async Task<bool> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("🔍 Validating environment...");

        // Simulate validation checks
        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

        var isValid = true;

        context.Logger.LogInformation("✓ Environment validation {Result}", isValid ? "passed" : "failed");

        return isValid;
    }
}
