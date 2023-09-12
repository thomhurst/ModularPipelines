using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class InstallNodeModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        return await context.Installer.PredefinedInstallers.Node();
    }
}