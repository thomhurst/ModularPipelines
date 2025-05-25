using ModularPipelines.Attributes;
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
    protected override async Task<bool> ShouldIgnoreFailures(IPipelineContext context, Exception exception)
    {
        var versionInformation = await GetModule<NugetVersionGeneratorModule>();

        return exception.Message.Contains($"tag 'v{versionInformation.Value!}' already exists");
    }

    protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var versionInformation = await GetModule<NugetVersionGeneratorModule>();

        await context.Git().Commands.Tag(new GitTagOptions
        {
            Arguments = [$"v{versionInformation.Value!}"],
        }, cancellationToken);

        return await context.Git().Commands.Push(new GitPushOptions
        {
            Tags = true,
        }, cancellationToken);
    }
}
