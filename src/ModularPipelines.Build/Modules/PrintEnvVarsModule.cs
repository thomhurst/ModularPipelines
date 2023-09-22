using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class PrintEnvVarsModule : Module
{
    protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        foreach (var (key, value) in context.Environment.EnvironmentVariables.GetEnvironmentVariables())
        {
            context.Logger.LogInformation("{Key}: {Value}", key, value);
        }

        return NothingAsync();
    }
}