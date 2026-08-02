using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class PrintGitInformationModule : Module<None>
{
    protected override async Task<None> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var gitInformation = await context.Git().Information.GetInfoAsync(cancellationToken);
        context.Logger.LogInformation("Git Info: {GitInfo}", JsonSerializer.Serialize(gitInformation, DiagnosticSerializerOptions.Instance));

        return None.Value;
    }
}
