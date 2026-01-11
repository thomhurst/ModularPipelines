using Microsoft.Extensions.Logging;
using ModularPipelines.FileSystem;
using ModularPipelines.Helpers;
using ModularPipelines.Http;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

internal class Downloader : IDownloader
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly IHttp _http;
    private readonly IFileSystemProvider _fileSystemProvider;

    public Downloader(IModuleLoggerProvider moduleLoggerProvider, IHttp http, IFileSystemProvider fileSystemProvider)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
        _http = http;
        _fileSystemProvider = fileSystemProvider;
    }

    public async Task<string?> DownloadStringAsync(DownloadOptions options,
        CancellationToken cancellationToken = default)
    {
        var response = await DownloadResponseAsync(options, cancellationToken).ConfigureAwait(false);

        return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<File> DownloadFileAsync(DownloadFileOptions options, CancellationToken cancellationToken = default)
    {
        var response = await DownloadResponseAsync(options, cancellationToken).ConfigureAwait(false);

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        await using (stream.ConfigureAwait(false))
        {
            var filePathToSave = GetSaveLocation(options);

            if (!options.Overwrite && _fileSystemProvider.FileExists(filePathToSave))
            {
                throw new IOException($"{filePathToSave} already exists and overwrite is false");
            }

            var newFile = _fileSystemProvider.Create(filePathToSave);
            await using (newFile.ConfigureAwait(false))
            {
                await stream.CopyToAsync(newFile, cancellationToken).ConfigureAwait(false);
            }

            _moduleLoggerProvider.GetLogger().LogInformation("File {Uri} downloaded to {SaveLocation}", options.DownloadUri, filePathToSave);

            return new File(filePathToSave, _fileSystemProvider);
        }
    }

    public async Task<HttpResponseMessage> DownloadResponseAsync(DownloadOptions options, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, options.DownloadUri);

        options.RequestConfigurator?.Invoke(request);

        var response = await _http.SendAsync(new HttpOptions(request)
        {
            LoggingType = options.LoggingType,
            HttpClient = options.HttpClient,
        }, cancellationToken).ConfigureAwait(false);

        return await response.EnsureSuccessStatusCodeWithContentAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Determines the file path where the downloaded content should be saved.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The save location is determined using the following logic:
    /// </para>
    /// <list type="number">
    /// <item><description>If SavePath is null/empty, generates a temp file path with a GUID name and extension from the download URI.</description></item>
    /// <item><description>If SavePath ends with a directory separator (e.g., "/" or "\"), treats it as a directory and generates a filename.</description></item>
    /// <item><description>If SavePath has a file extension, treats it as a complete file path.</description></item>
    /// <item><description>Otherwise, treats SavePath as a directory and generates a filename within it.</description></item>
    /// </list>
    /// <para>
    /// <strong>Note:</strong> The extension heuristic (step 3) may not be reliable for directory names
    /// containing dots (e.g., "my.folder"). To ensure a path is treated as a directory, append a
    /// directory separator character to the path.
    /// </para>
    /// </remarks>
    /// <param name="options">The download options containing the save path.</param>
    /// <returns>The determined file path for saving the download.</returns>
    private string GetSaveLocation(DownloadFileOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.SavePath))
        {
            return _fileSystemProvider.Combine(_fileSystemProvider.GetTempPath(), Guid.NewGuid() + GetExtension(options.DownloadUri.AbsoluteUri));
        }

        // Check if the path explicitly ends with a directory separator
        // This is a reliable indicator that the user intends this to be a directory
        if (PathHelpers.EndsWithDirectorySeparator(options.SavePath))
        {
            _fileSystemProvider.CreateDirectory(options.SavePath);
            return _fileSystemProvider.Combine(options.SavePath, Guid.NewGuid() + GetExtension(options.DownloadUri.AbsoluteUri));
        }

        // Use extension heuristic as a fallback
        // Note: This can be unreliable for directory names containing dots (e.g., "my.folder")
        if (Path.HasExtension(options.SavePath))
        {
            _fileSystemProvider.CreateDirectory(new FileInfo(options.SavePath).Directory!.FullName);
            return options.SavePath;
        }

        _fileSystemProvider.CreateDirectory(options.SavePath);
        return _fileSystemProvider.Combine(options.SavePath, Guid.NewGuid() + GetExtension(options.DownloadUri.AbsoluteUri));
    }

    private static string GetExtension(string downloadUri)
    {
        if (Path.HasExtension(downloadUri))
        {
            return Path.GetExtension(downloadUri);
        }

        return string.Empty;
    }
}
