using System.Net.Http.Headers;
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
[DependsOn<WaitForOtherOperatingSystemBuilds>]
public class DownloadCodeCoverageFromOtherOperatingSystemBuildsModule : Module<List<File>>
{
    private readonly IOptions<GitHubSettings> _gitHubSettings;

    public DownloadCodeCoverageFromOtherOperatingSystemBuildsModule(GitHubClient gitHubClient, IOptions<GitHubSettings> gitHubSettings, HttpClient httpClient)
    {
        _gitHubSettings = gitHubSettings;
    }
    
    protected override async Task<List<File>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var runs = await GetModule<WaitForOtherOperatingSystemBuilds>();

        var zipFiles = await runs.Value!.ToAsyncProcessorBuilder()
            .SelectAsync(run => context.Downloader.DownloadFileAsync(new DownloadFileOptions(new Uri($"{run.ArtifactsUrl}/zip"))
            {
                RequestConfigurator = message =>
                {
                    message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
                    message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _gitHubSettings.Value.StandardToken);
                }
            }, cancellationToken))
            .ProcessInParallel();

        return zipFiles.Select(x => context.Zip.UnZipToFolder(x, Folder.CreateTemporaryFolder()))
            .Select(x => x.FindFile(f => f.Name.Contains("coverage") && f.Extension == ".xml"))
            .OfType<File>()
            .ToList();
    }
}