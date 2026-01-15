using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class PrintEnvironmentVariablesModule : Module<IDictionary<string, object>>
{
    protected override Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Environment Variables: {EnvVars}", JsonSerializer.Serialize(context.Environment.Variables.GetEnvironmentVariables(), DiagnosticSerializerOptions.Instance));

        return Task.FromResult<IDictionary<string, object>?>(null);
    }
}