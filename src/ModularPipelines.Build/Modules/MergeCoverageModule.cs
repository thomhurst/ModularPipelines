using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Attributes;
using ModularPipelines.Build.Modules.Tests;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Extensions;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[RunOnLinux]
[SkipIfNoGitHubToken]
[SkipIfNoStandardGitHubToken]
[DependsOn<RunUnitTestsOnWindowsModule>]
[DependsOn<RunUnitTestsOnLinuxModule>]
[DependsOn<RunUnitTestsOnMacModule>]
public class MergeCoverageModule : Module<File>
{
    /// <inheritdoc/>
    protected override async Task<File?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        // Get coverage files from all three OS-specific test modules
        var windowsTests = await GetModule<RunUnitTestsOnWindowsModule>();
        var linuxTests = await GetModule<RunUnitTestsOnLinuxModule>();
        var macTests = await GetModule<RunUnitTestsOnMacModule>();

        var allCoverageFiles = new List<File>();

        if (windowsTests.Value?.CoverageFiles is { Count: > 0 })
        {
            allCoverageFiles.AddRange(windowsTests.Value.CoverageFiles);
            context.Logger.LogInformation("Added {Count} coverage files from Windows tests", windowsTests.Value.CoverageFiles.Count);
        }

        if (linuxTests.Value?.CoverageFiles is { Count: > 0 })
        {
            allCoverageFiles.AddRange(linuxTests.Value.CoverageFiles);
            context.Logger.LogInformation("Added {Count} coverage files from Linux tests", linuxTests.Value.CoverageFiles.Count);
        }

        if (macTests.Value?.CoverageFiles is { Count: > 0 })
        {
            allCoverageFiles.AddRange(macTests.Value.CoverageFiles);
            context.Logger.LogInformation("Added {Count} coverage files from Mac tests", macTests.Value.CoverageFiles.Count);
        }

        if (allCoverageFiles.Count == 0)
        {
            context.Logger.LogInformation("No coverage files found from any operating system");
            return null;
        }

        var coverageFiles = allCoverageFiles
            .Distinct()
            .ToList();

        await context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("dotnet")
        {
            Arguments = ["tool", "install", "--global", "dotnet-coverage"],
        }, cancellationToken);

        var outputPath = Folder.CreateTemporaryFolder().GetFile("cobertura.xml").Path;

        await context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("dotnet-coverage")
        {
            Arguments = new[] { "merge", "--remove-input-files", "--output-format", "cobertura", "--output", outputPath }.Concat(coverageFiles.Select(x => x.Path)),
        }, cancellationToken);

        return outputPath;
    }
}