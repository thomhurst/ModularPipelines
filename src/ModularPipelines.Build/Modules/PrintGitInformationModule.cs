using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class PrintGitInformationModule : Module<IDictionary<string, object>>
{
    public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        context.Logger.LogInformation("Git Info: {GitInfo}", JsonSerializer.Serialize(context.Git().Information, DiagnosticSerializerOptions.Instance));

        return null;
    }
}