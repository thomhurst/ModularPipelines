using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[RunOnLinuxOnly]
[ProducesArtifact("build-output", "../../_build-staging")]
public class BuildSolutionsModule : Module<CommandResult[]>
{
    private static readonly string[] Solutions =
    [
        "ModularPipelines.Analyzers.sln",
        "ModularPipelines.All.sln",
        "ModularPipelines.Examples.sln",
        "src/ModularPipelines.Azure/ModularPipelines.Azure.sln",
        "src/ModularPipelines.AmazonWebServices/ModularPipelines.AmazonWebServices.sln",
        "src/ModularPipelines.Google/ModularPipelines.Google.sln",
    ];

    protected override async Task<CommandResult[]?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var gitRoot = context.Git().RootDirectory.Path;

        // Build all solutions with --no-restore (workflow already restored).
        // -maxcpucount:1 forces a serial build (one project compiled at a time). Building the
        // 60+ project ModularPipelines.All.sln with default parallelism is CPU- and memory-heavy
        // enough to get the CI runner reclaimed mid-build (#3179); serialising keeps peak CPU and
        // memory low. Slower, but the pipeline is standalone on a single runner now.
        var results = await Solutions
            .ToAsyncProcessorBuilder()
            .SelectAsync(async solution => await context.DotNet().Build(new DotNetBuildOptions
            {
                ProjectSolution = Path.Combine(gitRoot, solution),
                Configuration = "Release",
                NoRestore = true,
                Arguments = ["-maxcpucount:1"],
            }, cancellationToken: cancellationToken))
            .ProcessOneAtATime();

        // Stage bin/Release/ output for artifact sharing
        var stagingDir = Path.Combine(gitRoot, "_build-staging");

        if (Directory.Exists(stagingDir))
        {
            Directory.Delete(stagingDir, recursive: true);
        }

        foreach (var binDir in Directory.EnumerateDirectories(gitRoot, "bin", SearchOption.AllDirectories))
        {
            // Skip the staging directory itself
            if (binDir.StartsWith(stagingDir, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            // Skip the pipeline app's own output (already built by dotnet run)
            if (binDir.Contains(Path.Combine("ModularPipelines.Build", "bin"), StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var releaseDir = Path.Combine(binDir, "Release");
            if (!Directory.Exists(releaseDir))
            {
                continue;
            }

            // Compute repo-relative path and create staging destination
            var relativeBinDir = Path.GetRelativePath(gitRoot, binDir);
            var stagingDest = Path.Combine(stagingDir, relativeBinDir, "Release");

            CopyDirectory(releaseDir, stagingDest);
        }

        return results;
    }

    private static void CopyDirectory(string sourceDir, string destDir)
    {
        Directory.CreateDirectory(destDir);

        foreach (var file in Directory.GetFiles(sourceDir))
        {
            File.Copy(file, Path.Combine(destDir, Path.GetFileName(file)), overwrite: true);
        }

        foreach (var dir in Directory.GetDirectories(sourceDir))
        {
            CopyDirectory(dir, Path.Combine(destDir, Path.GetFileName(dir)));
        }
    }
}
