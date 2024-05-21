using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Attributes;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.Modules;
using Octokit;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[RunOnLinux]
[SkipIfNoGitHubToken]
[SkipIfNoStandardGitHubToken]
[DependsOn<WaitForOtherOperatingSystemBuilds>]
public class DownloadCodeCoverageFromOtherOperatingSystemBuildsModule : Module<List<File>>
{
    private readonly IGitHubClient _gitHubClient;

    public DownloadCodeCoverageFromOtherOperatingSystemBuildsModule(IGitHubClient gitHubClient)
    {
        _gitHubClient = gitHubClient;
    }

    /// <inheritdoc/>
    protected override async Task<List<File>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var runs = await GetModule<WaitForOtherOperatingSystemBuilds>();

        if (runs.Value?.Any() != true)
        {
            return new List<File>();
        }

        var artifacts = await runs.Value!.ToAsyncProcessorBuilder()
            .SelectAsync(async run =>
            {
                var listWorkflowArtifacts = await _gitHubClient.Actions.Artifacts.ListWorkflowArtifacts(BuildConstants.Owner,
                    BuildConstants.RepositoryName, run.Id);

                return listWorkflowArtifacts.Artifacts.FirstOrDefault(x => x.Name == "code-coverage") ?? throw new ArgumentException("No code-coverage artifact found");
            })
            .ProcessInParallel();

        var zipFiles = await artifacts
            .ToAsyncProcessorBuilder()
            .SelectAsync(DownloadZip)
            .ProcessInParallel();

        return zipFiles.Select(x => context.Zip.UnZipToFolder(x, Folder.CreateTemporaryFolder()))
            .SelectMany(x => x.GetFiles(f => f.Extension == ".xml" && f.Name.Contains("cobertura")))
            .ToList();
    }

    private async Task<File> DownloadZip(Artifact artifact)
    {
        var zipStream = await _gitHubClient.Actions.Artifacts.DownloadArtifact(BuildConstants.Owner,
            BuildConstants.RepositoryName,
            artifact.Id, "zip");

        if (zipStream is null)
        {
            throw new Exception($"Stream from artifact {artifact.Id} is null");
        }

        var file = File.GetNewTemporaryFilePath();

        await file.WriteAsync(zipStream);

        return file;
    }
}
