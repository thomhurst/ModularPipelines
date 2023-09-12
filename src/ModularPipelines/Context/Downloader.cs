using Microsoft.Extensions.Logging;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

internal class Downloader : IDownloader
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly HttpClient _defaultHttpClient;
    private readonly ILogger _logger;

    public Downloader(IModuleLoggerProvider moduleLoggerProvider, HttpClient defaultHttpClient)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
        _logger = moduleLoggerProvider.GetLogger();
        _defaultHttpClient = defaultHttpClient;
    }

    public async Task<string?> DownloadStringAsync(DownloadOptions options,
        CancellationToken cancellationToken = default)
    {
        var response = await DownloadResponseAsync(options, cancellationToken);

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    public async Task<File> DownloadFileAsync(DownloadFileOptions options, CancellationToken cancellationToken = default)
    {
        var response = await DownloadResponseAsync(options, cancellationToken);

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var filePathToSave = GetSaveLocation(options);

        if (!options.Overwrite && System.IO.File.Exists(filePathToSave))
        {
            throw new IOException($"{filePathToSave} already exists and overwrite is false");
        }

        await using var newFile = System.IO.File.Create(filePathToSave);

        await stream.CopyToAsync(newFile, cancellationToken);

        _moduleLoggerProvider.GetLogger().LogInformation("File {Uri} downloaded to {SaveLocation}", options.DownloadUri, filePathToSave);

        return filePathToSave!;
    }

    public async Task<HttpResponseMessage> DownloadResponseAsync(DownloadOptions options, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, options.DownloadUri);

        options.RequestConfigurator?.Invoke(request);

        var response = await (options.HttpClient ?? _defaultHttpClient).GetAsync(options.DownloadUri, cancellationToken);

        return response.EnsureSuccessStatusCode();
    }

    private string GetSaveLocation(DownloadFileOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.SavePath))
        {
            return Path.Combine(Path.GetTempPath(), Guid.NewGuid() + GetExtension(options.DownloadUri.AbsoluteUri));
        }

        if (Path.HasExtension(options.SavePath))
        {
            Directory.CreateDirectory(new FileInfo(options.SavePath).Directory!.FullName);
            return options.SavePath;
        }

        Directory.CreateDirectory(options.SavePath);
        return Path.Combine(options.SavePath, Guid.NewGuid() + GetExtension(options.DownloadUri.AbsoluteUri));
    }

    private string GetExtension(string downloadUri)
    {
        if (Path.HasExtension(downloadUri))
        {
            return Path.GetExtension(downloadUri);
        }

        return string.Empty;
    }
}
