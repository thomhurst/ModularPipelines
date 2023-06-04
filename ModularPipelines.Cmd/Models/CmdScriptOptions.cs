using ModularPipelines.Command.Options;

namespace ModularPipelines.Cmd.Models;

public record CmdScriptOptions(string Script) : CommandEnvironmentOptions
{
    public bool Echo { get; init; } = true;
}