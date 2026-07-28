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
    protected override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return await context.DotNet().Build(new DotNetBuildOptions
        {
            ProjectSolution = Path.Combine(context.Git().RootDirectory.Path, "ModularPipelines.All.sln"),
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
