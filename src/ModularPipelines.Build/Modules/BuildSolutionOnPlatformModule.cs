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
            ProjectSolution = Path.Combine(context.Git().RootDirectory.Path, "ModularPipelines.sln"),
            Configuration = "Release",
            NoRestore = true,
        }, cancellationToken: cancellationToken);
    }
}

[RequiresCapability("windows")]
public sealed class BuildSolutionOnWindowsModule : BuildSolutionOnPlatformModule
{
}

[RequiresCapability("macos")]
public sealed class BuildSolutionOnMacOSModule : BuildSolutionOnPlatformModule
{
}
