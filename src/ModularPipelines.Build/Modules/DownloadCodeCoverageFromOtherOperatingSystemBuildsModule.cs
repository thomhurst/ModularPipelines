using System.Net.Http.Headers;
using System.Net.Http.Json;
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

[RunOnLinux, SkipIfDependabot]
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

        if (runs.Value?.Any() != true)
        {
            return new List<File>();
        }

        var httpResponses = await runs.Value!.ToAsyncProcessorBuilder()
            .SelectAsync(run => context.Http.SendAsync(new HttpRequestMessage(HttpMethod.Get, new Uri($"{run.ArtifactsUrl}"))
            {
                Headers =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/vnd.github+json") },
                    Authorization = new AuthenticationHeaderValue("Bearer", _gitHubSettings.Value.StandardToken),
                    UserAgent = { new ProductInfoHeaderValue("ModularPipelines", "1.0.0") }
                }
            }, cancellationToken))
            .ProcessInParallel();

        var models = await httpResponses.ToAsyncProcessorBuilder()
            .SelectAsync(message => message.Content.ReadFromJsonAsync<GitHubArtifactsList>(cancellationToken: cancellationToken))
            .ProcessInParallel();
        
        var zipFiles = await models.OfType<GitHubArtifactsList>()
            .Select(list => list.Artifacts!.First(x => x.Name == "code-coverage"))
            .ToAsyncProcessorBuilder()
            .SelectAsync(artifact => context.Downloader.DownloadFileAsync(new DownloadFileOptions(artifact.DownloadUrl!)
            {
                RequestConfigurator = message =>
                {
                    message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
                    message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _gitHubSettings.Value.StandardToken);
                    message.Headers.UserAgent.Add(new ProductInfoHeaderValue("ModularPipelines", "1.0.0"));
                }
            }, cancellationToken))
            .ProcessInParallel();

        return zipFiles.Select(x => context.Zip.UnZipToFolder(x, Folder.CreateTemporaryFolder()))
            .Select(x => x.FindFile(f => f.Name.Contains("coverage") && f.Extension == ".xml"))
            .OfType<File>()
            .ToList();
    }
}