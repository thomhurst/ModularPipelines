using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Git.Attributes;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[RunOnlyOnBranch("main")]
[RunOnLinuxOnly]
[DependsOn<NugetVersionGeneratorModule>]
public class PushVersionTagModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithIgnoreFailuresWhen((ctx, ex) =>
        {
            var versionInformation = ((IModuleContext)ctx).GetModule<NugetVersionGeneratorModule, string>();
            return ex.Message.Contains($"tag 'v{versionInformation.ValueOrDefault!}' already exists");
        })
        .Build();

    protected override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var versionInformation = context.GetModule<NugetVersionGeneratorModule, string>();

        await context.Git().Commands.Tag(new GitTagOptions
        {
            TagName = $"v{versionInformation.ValueOrDefault!}",
        }, token: cancellationToken);

        return await context.Git().Commands.Push(new GitPushOptions
        {
            Tags = true,
        }, token: cancellationToken);
    }
}
