namespace ModularPipelines.Powershell.Models;

public record PowershellFileOptions(string FilePath) : PowershellOptions
{
    public IEnumerable<string>? Arguments { get; init; }
}
