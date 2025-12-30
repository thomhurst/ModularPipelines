using ModularPipelines.Options;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

public interface IDownloader
{
    /// <summary>
    /// Downloads a file from the web and saves it to the local machine.
    /// </summary>
    /// <param name="options">The download options including the source URL and destination path.</param>
    /// <param name="cancellationToken">A token to cancel the download operation.</param>
    /// <returns>The downloaded file.</returns>
    public Task<File> DownloadFileAsync(DownloadFileOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads content from the web as a string.
    /// </summary>
    /// <param name="options">The download options including the source URL.</param>
    /// <param name="cancellationToken">A token to cancel the download operation.</param>
    /// <returns>The downloaded content as a string, or null if the download failed.</returns>
    public Task<string?> DownloadStringAsync(DownloadOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an HTTP response from the web.
    /// </summary>
    /// <param name="options">The download options including the source URL.</param>
    /// <param name="cancellationToken">A token to cancel the download operation.</param>
    /// <returns>The HTTP response message.</returns>
    public Task<HttpResponseMessage> DownloadResponseAsync(DownloadOptions options, CancellationToken cancellationToken = default);
}
