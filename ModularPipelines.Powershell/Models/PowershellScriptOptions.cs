using ModularPipelines.Options;

namespace ModularPipelines.Powershell.Models;

public record PowershellScriptOptions(string Script) : CommandLineOptions
{
    public IEnumerable<string>? Arguments { get; init; }
}