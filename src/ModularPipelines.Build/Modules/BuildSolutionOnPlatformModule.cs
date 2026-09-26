using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public abstract class BuildSolutionOnPlatformModule : Module<CommandResult[]>
{
    // The macOS baseline takes about 56 minutes. Keep compilation bounded below
    // the coordinator's 65-minute result timeout and the 90-minute job timeout.
    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithTimeout(TimeSpan.FromMinutes(60));

    protected override async Task<CommandResult[]> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var repositoryInfo = await context.Tools.Git.Information.GetInfoAsync(cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException("Git repository information is unavailable.");

        var solutions = File.ReadLines(Path.Combine(repositoryInfo.Root.Path, "BuildSolutions.txt"))
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrEmpty(line) && !line.StartsWith('#'));
        var results = new List<CommandResult>();
        foreach (var solution in solutions)
        {
            results.Add(await context.Tools.DotNet.BuildAsync(new DotNetBuildOptions
            {
                ProjectSolution = Path.Combine(repositoryInfo.Root.Path, solution),
                Configuration = "Release",
                NoRestore = true,
                Arguments = ["/m:1", "-p:UseSharedCompilation=false"],
            }, cancellationToken: cancellationToken).ConfigureAwait(false));
        }

        return [.. results];
    }
}

[RunIf<ModularPipelines.OnWindows>]
public sealed class BuildSolutionOnWindowsModule : BuildSolutionOnPlatformModule
{
}

[RunIf<ModularPipelines.OnMacOS>]
public sealed class BuildSolutionOnMacOSModule : BuildSolutionOnPlatformModule
{
}
