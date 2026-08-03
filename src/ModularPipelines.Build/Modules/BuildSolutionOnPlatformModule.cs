using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public abstract class BuildSolutionOnPlatformModule : Module<CommandResult>
{
    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var repositoryInfo = await context.Git().Information.GetInfoAsync().ConfigureAwait(false)
            ?? throw new InvalidOperationException("Git repository information is unavailable.");

        return await context.DotNet().BuildAsync(new DotNetBuildOptions
        {
            ProjectSolution = Path.Combine(repositoryInfo.Root.Path, "ModularPipelines.All.slnx"),
            Configuration = "Release",
            NoRestore = true,
        }, cancellationToken: cancellationToken);
    }
}

[RunIfAll<ModularPipelines.Conditions.OnWindows>]
public sealed class BuildSolutionOnWindowsModule : BuildSolutionOnPlatformModule
{
}

[RunIfAll<ModularPipelines.Conditions.OnMacOS>]
public sealed class BuildSolutionOnMacOSModule : BuildSolutionOnPlatformModule
{
}
