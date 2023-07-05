using ModularPipelines.Options;

namespace ModularPipelines.Cmd.Models;

public record CmdScriptOptions(string Script) : CommandLineOptions
{
    public bool Echo { get; init; } = true;
}
