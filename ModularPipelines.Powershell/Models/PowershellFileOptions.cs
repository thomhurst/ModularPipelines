using ModularPipelines.Options;

namespace ModularPipelines.Powershell.Models;

public record PowershellFileOptions(string FilePath) : CommandEnvironmentOptions
{
    public IEnumerable<string>? Arguments { get; init; }
}