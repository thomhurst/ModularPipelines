using ModularPipelines.Options;

namespace ModularPipelines.Powershell.Models;

public record PowershellScriptOptions(string Script) : CommandEnvironmentOptions
{
    public IEnumerable<string>? Arguments { get; init; }
}