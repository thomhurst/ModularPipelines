using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Cmd.Models;

public record CmdScriptOptions([property: PositionalArgument] string Script) : CommandLineOptions
{
    public bool Echo { get; init; } = true;
}
