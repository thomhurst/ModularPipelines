using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Cmd.Models;

[ExcludeFromCodeCoverage]
public record CmdScriptOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Script) : CommandLineToolOptions("cmd")
{
    [BooleanCommandSwitch("/q")]
    public bool DisableEcho { get; init; }

    [BooleanCommandSwitch("/c")]
    public bool StopAfter { get; init; } = true;

    [BooleanCommandSwitch("/u")]
    public bool Unicode { get; init; }

    [BooleanCommandSwitch("/a")]
    public bool Ansi { get; init; }

    [BooleanCommandSwitch("/d")]
    public bool DisableAutoRunCommands { get; init; }
}
