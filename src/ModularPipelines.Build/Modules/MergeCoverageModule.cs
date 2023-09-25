using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[RunOnLinux]
[SkipIfDependabot]
[DependsOn<DownloadCodeCoverageFromOtherOperatingSystemBuildsModule>]
[DependsOn<RunUnitTestsModule>]
public class MergeCoverageModule : Module<File>
{
    /// <inheritdoc/>
    protected override async Task<File?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var coverageFilesFromThisRun = context.Git().RootDirectory
            .GetFiles(x => x.Name.Contains("coverage") && x.Extension == ".xml");

        var coverageFilesFromOtherSystems = await GetModule<DownloadCodeCoverageFromOtherOperatingSystemBuildsModule>();

        if (coverageFilesFromOtherSystems.Value?.Any() != true)
        {
            return null;
        }

        var coverageFiles = coverageFilesFromOtherSystems.Value!
            .Concat(coverageFilesFromThisRun)
            .Distinct()
            .ToList();

        await context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("dotnet")
        {
            Arguments = new[] { "tool", "install", "--global", "dotnet-coverage" },
        }, cancellationToken);

        var outputPath = Folder.CreateTemporaryFolder().GetFile("coverage.cobertura.xml").Path;

        await context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("dotnet-coverage")
        {
            Arguments = new[] { "merge", "--remove-input-files", "--output-format", "cobertura", "--output", outputPath }.Concat(coverageFiles.Select(x => x.Path)),
        }, cancellationToken);

        return outputPath;
    }
}