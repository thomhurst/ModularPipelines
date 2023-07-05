using ModularPipelines.Options;

namespace ModularPipelines.Powershell.Models;

public record PowershellFileOptions( string FilePath ) : CommandLineOptions
{
    public IEnumerable<string>? Arguments { get; init; }
}
