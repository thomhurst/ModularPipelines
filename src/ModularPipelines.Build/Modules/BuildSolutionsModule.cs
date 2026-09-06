using EnumerableAsyncProcessor.Extensions;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[RunIf<ModularPipelines.OnLinux>]
[ProducesArtifact("build-output", "../../_build-staging")]
public class BuildSolutionsModule(IOptions<PipelineSettings> pipelineSettings) : Module<CommandResult[]>
{
    protected override async Task<CommandResult[]> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var repositoryInfo = await context.Tools.Git.Information.GetInfoAsync().ConfigureAwait(false)
            ?? throw new InvalidOperationException("Git repository information is unavailable.");
        var gitRoot = repositoryInfo.Root.Path;
        // CI builds before starting this executable. Even an incremental pass evaluates
        // every project again, so reuse that build while still completing this dependency.
        CommandResult[] results = [];
        if (!pipelineSettings.Value.BuildAlreadyCompleted)
        {
            var solutions = File.ReadLines(Path.Combine(gitRoot, "BuildSolutions.txt"))
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line) && !line.StartsWith('#'))
                .ToArray();

            results = await solutions
                .ToAsyncProcessorBuilder()
                .SelectAsync(async solution => await context.Tools.DotNet.BuildAsync(new DotNetBuildOptions
                {
                    ProjectSolution = Path.Combine(gitRoot, solution),
                    Configuration = "Release",
                    NoRestore = true,
                }, cancellationToken: cancellationToken))
                .ProcessOneAtATime();
        }

        if (!context.Services.GetRequiredService<BuildOutputSharing>().IsEnabled)
        {
            return results;
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
