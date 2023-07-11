using ModularPipelines.Options;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

public interface IDownloader
{
    /// <summary>
    /// Used to download a file from the web
    /// </summary>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<File> DownloadFileAsync(DownloadOptions options, CancellationToken cancellationToken = default);
}
