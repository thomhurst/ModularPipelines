using ModularPipelines.Options;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

public interface IDownloader
{
    /// <summary>
    /// Used to download a file from the web and save to the local machine
    /// </summary>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<File> DownloadFileAsync(DownloadFileOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Used to download a string from the web
    /// </summary>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<string?> DownloadStringAsync(DownloadOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Used to get a response from the web
    /// </summary>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<HttpResponseMessage> DownloadResponseAsync(DownloadOptions options, CancellationToken cancellationToken = default);
}
