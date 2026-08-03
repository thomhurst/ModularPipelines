using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using TemplatePipeline.Settings;

namespace TemplatePipeline.Modules;

[DependsOn<RestoreModule>]
public sealed class BuildModule(IOptions<BuildSettings> settings) : Module<CommandResult>
{
    protected override async Task<CommandResult> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Tools.DotNet.BuildAsync(
            new DotNetBuildOptions
            {
                ProjectSolution = settings.Value.Solution,
                Configuration = settings.Value.Configuration,
                NoRestore = true,
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
