namespace ModularPipelines.Options;

public record InstallerOptions(string Path)
{
    public IEnumerable<string>? Arguments { get; init; }
}
