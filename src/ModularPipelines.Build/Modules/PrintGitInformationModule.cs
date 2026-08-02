using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class PrintGitInformationModule : Module<IDictionary<string, object>>
{
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var gitInformation = await context.Git().Information.GetInfoAsync(cancellationToken);
        context.Logger.LogInformation("Git Info: {GitInfo}", JsonSerializer.Serialize(gitInformation, DiagnosticSerializerOptions.Instance));

        return null;
    }
}
