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
    protected override Task<None> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Git Info: {GitInfo}", JsonSerializer.Serialize(context.Git().Information, DiagnosticSerializerOptions.Instance));

        return Task.FromResult(None.Value);
    }
}
