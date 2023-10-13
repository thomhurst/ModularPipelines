using ModularPipelines.Http;

namespace ModularPipelines.Options;

public record DownloadFileOptions : DownloadOptions
{
    public DownloadFileOptions(Uri downloadUri) : base(downloadUri)
    {
        LoggingType = HttpLoggingType.Request | HttpLoggingType.StatusCode | HttpLoggingType.Duration;
    }

    public string? SavePath { get; init; }

    public bool Overwrite { get; init; } = true;
}