using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[RunIf<ModularPipelines.OnLinux>]
[ProducesArtifact("build-output", "../../_build-staging")]
public class BuildSolutionsModule : Module<CommandResult[]>
{
    protected override async Task<CommandResult[]> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var repositoryInfo = await context.Tools.Git.Information.GetInfoAsync().ConfigureAwait(false)
            ?? throw new InvalidOperationException("Git repository information is unavailable.");
        var gitRoot = repositoryInfo.Root.Path;
        var solutions = File.ReadLines(Path.Combine(gitRoot, "BuildSolutions.txt"))
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrEmpty(line) && !line.StartsWith('#'))
            .ToArray();

        // Build all solutions with --no-restore (the workflow already restored and, in CI,
        // natively built them, so this is a fast MSBuild-incremental pass). Default
        // parallelism: the runner reclaim in #3179 came from a single test project that
        // referenced every integration at once, pulling the huge AWS/Azure/Google SDK metadata
        // into one compilation unit; that test was removed, not MSBuild parallelism changed.
        var results = await solutions
            .ToAsyncProcessorBuilder()
            .SelectAsync(async solution => await context.Tools.DotNet.BuildAsync(new DotNetBuildOptions
            {
                ProjectSolution = Path.Combine(gitRoot, solution),
                Configuration = "Release",
                NoRestore = true,
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
