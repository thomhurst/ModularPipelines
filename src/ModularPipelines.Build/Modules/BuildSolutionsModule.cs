using Microsoft.Extensions.Logging;
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

        // Build all solutions serially with --no-restore (workflow already restored).
        // Log each solution before/after so that if the job dies mid-build (e.g. the
        // master runner being reclaimed during the heavy All.sln build - see #3179) the
        // log shows exactly which solution was building, instead of an opaque silent gap.
        // Command output is buffered until completion, so these module-level logs are the
        // reliable progress signal.
        var results = new List<CommandResult>(Solutions.Length);
        for (var index = 0; index < Solutions.Length; index++)
        {
            var solution = Solutions[index];
            context.Logger.LogInformation(
                "Building solution {Number}/{Total}: {Solution}", index + 1, Solutions.Length, solution);

            var result = await context.DotNet().Build(new DotNetBuildOptions
            {
                ProjectSolution = Path.Combine(gitRoot, solution),
                Configuration = "Release",
                NoRestore = true,
            }, cancellationToken: cancellationToken);

            context.Logger.LogInformation(
                "Finished solution {Number}/{Total}: {Solution} (exit code {ExitCode})",
                index + 1, Solutions.Length, solution, result.ExitCode);

            results.Add(result);
        }

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

        return results.ToArray();
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
