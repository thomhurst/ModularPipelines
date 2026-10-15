using System.Net;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.DocumentationSnippets;

// docs/docs/how-to/run-conditions.md
public class ServiceIsAvailable : IRunCondition
{
    public async Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        using var response = await context.Network.Http.SendAsync(
            new HttpOptions(new HttpRequestMessage(HttpMethod.Get, "https://www.example.com/ping")),
            cancellationToken);
        return response.StatusCode == HttpStatusCode.OK;
    }
}

// docs/docs/how-to/cancellation-tokens.md
public class MyModule : Module<FilePath>
{
    protected override async Task<FilePath> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return await context.Network.Downloader.DownloadFileAsync(new DownloadFileOptions(new Uri("https://www.example.com/somefile.zip")), cancellationToken);
    }
}

// docs/docs/how-to/defining-modules.md
public class FindAFileModule : Module<FilePath>
{
    protected override Task<FilePath> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        var file = context.Files
            .Glob("C:\\**\\MyJsonFile.json")
            .Single();
        return Task.FromResult(file);
    }
}
