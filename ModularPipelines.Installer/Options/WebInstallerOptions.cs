namespace ModularPipelines.Installer.Options;

public record WebInstallerOptions(Uri DownloadUri)
{
    public IEnumerable<string>? Arguments { get; init; }
}