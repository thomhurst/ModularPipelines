using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Cmd.Models;

public record CmdScriptOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Script) : CommandLineToolOptions("cmd")
{
    [BooleanCommandSwitch("/q")]
    public bool DisableEcho { get; init; } = false;

    [BooleanCommandSwitch("/c")]
    public bool StopAfter { get; init; } = true;

    [BooleanCommandSwitch("/u")]
    public bool Unicode { get; init; } = false;

    [BooleanCommandSwitch("/a")]
    public bool Ansi { get; init; } = false;

    [BooleanCommandSwitch("/d")]
    public bool DisableAutoRunCommands { get; init; } = false;
}
