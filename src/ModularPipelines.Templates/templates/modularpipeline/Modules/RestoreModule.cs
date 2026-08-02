using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using TemplatePipeline.Settings;

namespace TemplatePipeline.Modules;

public sealed class RestoreModule(IOptions<BuildSettings> settings) : Module<CommandResult>
{
    protected override async Task<CommandResult> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Tools.DotNet.RestoreAsync(
            new DotNetRestoreOptions
            {
                ProjectSolution = settings.Value.Solution,
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
