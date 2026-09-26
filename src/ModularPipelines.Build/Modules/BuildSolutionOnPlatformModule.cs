using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Build.Modules;

public abstract class BuildSolutionOnPlatformModule : Module<CommandResult[]>
{
    // Full macOS compilation can exceed an hour. The same budget applies to
    // commands and the enclosing module, leaving 15 minutes below the job timeout.
    private static readonly TimeSpan BuildTimeout = TimeSpan.FromMinutes(75);

    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithTimeout(BuildTimeout);

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
            }, new CommandExecutionOptions
            {
                ExecutionTimeout = BuildTimeout,
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
