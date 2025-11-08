using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Attributes;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;

namespace ModularPipelines.Build.Modules;

[RunOnlyOnBranch("main")]
[RunOnLinuxOnly]
[DependsOn<NugetVersionGeneratorModule>]
public class PushVersionTagModule : ModuleNew<CommandResult>, IModuleErrorHandling
{
    public async Task<bool> ShouldIgnoreFailuresAsync(IPipelineContext context, Exception exception)
    {
        var versionInformation = await context.GetModuleAsync<NugetVersionGeneratorModule>();

        return exception.Message.Contains($"tag 'v{versionInformation.Value!}' already exists");
    }

    public override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var versionInformation = await context.GetModuleAsync<NugetVersionGeneratorModule>();

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
