using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[RunOnLinux]
[DependsOn<DownloadCodeCoverageFromOtherOperatingSystemBuildsModule>]
[DependsOn<RunUnitTestsModule>]
public class MergeCoverageModule : Module<File>
{
    protected override async Task<File?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var coverageFilesFromThisRun = context.Git().RootDirectory
            .GetFiles(x => x.Name.Contains("coverage") && x.Extension == ".xml");
        var coverageFilesFromOtherSystems = await GetModule<DownloadCodeCoverageFromOtherOperatingSystemBuildsModule>();

        var coverageFiles = coverageFilesFromOtherSystems.Value!
            .Concat(coverageFilesFromThisRun)
            .Distinct()
            .ToList();

        await context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("dotnet")
        {
            Arguments = new[] { "tool", "install", "--global", "dotnet-coverage" }
        }, cancellationToken);

        var outputPath = File.GetNewTemporaryFilePath();
        
        await context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("dotnet-coverage")
        {
            Arguments = new[] { "merge", "--remove-input-files", "--output", outputPath.Path }.Concat(coverageFiles.Select(x => x.Path))
        }, cancellationToken);

        return outputPath;
    }
}