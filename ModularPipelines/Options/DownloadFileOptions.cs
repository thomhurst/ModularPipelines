namespace ModularPipelines.Options;

public record DownloadFileOptions(Uri DownloadUri) : DownloadOptions(DownloadUri)
{
    public string? SavePath { get; init; }
    public bool Overwrite { get; init; } = true;
}