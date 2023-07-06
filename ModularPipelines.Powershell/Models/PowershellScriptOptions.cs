namespace ModularPipelines.Powershell.Models;

public record PowershellScriptOptions(string Script) : PowershellOptions
{
    public IEnumerable<string>? Arguments { get; init; }
}