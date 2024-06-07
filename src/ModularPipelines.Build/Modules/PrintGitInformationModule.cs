using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class PrintGitInformationModule : Module
{
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        
        context.Logger.LogInformation("Environment Variables: {EnvVars}", JsonSerializer.Serialize(context.Git().Information));
        
        return null;
    }
}