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
public class PushVersionTagModule : IModule<CommandResult>, IIgnoreFailures
{
    public Task<bool> ShouldIgnoreFailures(IPipelineContext context, Exception exception)
    {
        var versionInformation = ((IModuleContext)context).GetModule<NugetVersionGeneratorModule, string>();

        return Task.FromResult(exception.Message.Contains($"tag 'v{versionInformation.Value!}' already exists"));
    }

    public async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var versionInformation = context.GetModule<NugetVersionGeneratorModule, string>();

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
