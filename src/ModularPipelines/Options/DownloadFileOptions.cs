using ModularPipelines.Http;

namespace ModularPipelines.Options;

public record DownloadFileOptions : DownloadOptions
{
    public DownloadFileOptions(Uri DownloadUri) : base(DownloadUri)
    {
        LoggingType = HttpLoggingType.RequestOnly;
    }

    public string? SavePath { get; init; }
    public bool Overwrite { get; init; } = true;
}