using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using TemplatePipeline.Settings;

namespace TemplatePipeline.Modules;

[DependsOn<BuildModule>]
public sealed class TestModule(IOptions<BuildSettings> settings) : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.DotNet().Test(
            new DotNetTestOptions
            {
                Solution = settings.Value.Solution,
                Configuration = settings.Value.Configuration,
                NoBuild = true,
                NoRestore = true,
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
