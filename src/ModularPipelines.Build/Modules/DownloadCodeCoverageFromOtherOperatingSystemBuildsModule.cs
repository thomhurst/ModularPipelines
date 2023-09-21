using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Octokit;
using TomLonghurst.EnumerableAsyncProcessor.Extensions;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[RunOnLinux]
[DependsOn<TriggerOtherOperatingSystemBuilds>]
public class DownloadCodeCoverageFromOtherOperatingSystemBuildsModule : Module<List<File>>
{
    private readonly GitHubClient _gitHubClient;
    private readonly IOptions<GitHubSettings> _gitHubSettings;
    private readonly HttpClient _httpClient;

    public DownloadCodeCoverageFromOtherOperatingSystemBuildsModule(GitHubClient gitHubClient, IOptions<GitHubSettings> gitHubSettings, HttpClient httpClient)
    {
        _gitHubClient = gitHubClient;
        _gitHubSettings = gitHubSettings;
        _httpClient = httpClient;
    }
    
    protected override async Task<List<File>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var runs = await GetModule<TriggerOtherOperatingSystemBuilds>();

        var zipFiles = await runs.Value!.ToAsyncProcessorBuilder()
            .SelectAsync(run => context.Downloader.DownloadFileAsync(new DownloadFileOptions(new Uri($"{run.ArtifactsUrl}/zip")), cancellationToken))
            .ProcessInParallel();

        return zipFiles.Select(x => context.Zip.UnZipToFolder(x, Folder.CreateTemporaryFolder()))
            .Select(x => x.FindFile(f => f.Name.Contains("coverage") && f.Extension == ".xml"))
            .OfType<File>()
            .ToList();
    }
}