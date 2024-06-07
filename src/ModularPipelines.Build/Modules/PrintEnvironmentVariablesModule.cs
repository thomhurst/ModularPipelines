using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class PrintEnvironmentVariablesModule : Module
{
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        
        context.Logger.LogInformation("Environment Variables: {EnvVars}", JsonSerializer.Serialize(context.Environment.EnvironmentVariables.GetEnvironmentVariables()));
        
        return null;
    }
}