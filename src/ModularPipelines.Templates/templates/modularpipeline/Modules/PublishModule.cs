using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using TemplatePipeline.Settings;

namespace TemplatePipeline.Modules;

[DependsOn<TestModule>]
public sealed class PublishModule(IOptions<BuildSettings> settings) : Module<CommandResult>
{
    protected override async Task<CommandResult> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.Tools.DotNet.PublishAsync(
            new DotNetPublishOptions
            {
                ProjectSolution = settings.Value.PublishProject,
                Configuration = settings.Value.Configuration,
                Output = settings.Value.PublishDirectory,
                NoBuild = true,
                NoRestore = true,
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
