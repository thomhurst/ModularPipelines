using ModularPipelines.Http;

namespace ModularPipelines.Options;

/// <summary>
/// Options for downloading a file to the local file system.
/// </summary>
public record DownloadFileOptions : DownloadOptions
{
    /// <summary>
    /// Initialises a new instance of the <see cref="DownloadFileOptions"/> class.
    /// Initializes a new instance of the <see cref="DownloadFileOptions"/> class.
    /// </summary>
    /// <param name="downloadUri">The URI to download from.</param>
    public DownloadFileOptions(Uri downloadUri) : base(downloadUri)
    {
        LoggingType = HttpLoggingType.Request | HttpLoggingType.StatusCode | HttpLoggingType.Duration;
    }

    /// <summary>
    /// Gets the path where the downloaded file should be saved. If not specified, a temporary file will be created.
    /// </summary>
    public string? SavePath { get; init; }

    /// <summary>
    /// Gets a value indicating whether to overwrite the file if it already exists.
    /// </summary>
    public bool Overwrite { get; init; } = true;
}