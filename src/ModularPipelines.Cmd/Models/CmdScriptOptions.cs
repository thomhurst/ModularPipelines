using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Cmd.Models;

[ExcludeFromCodeCoverage]
public record CmdScriptOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Script) : CommandLineToolOptions("cmd")
{
    [BooleanCommandSwitch("/q")]
    public virtual bool DisableEcho { get; init; }

    [BooleanCommandSwitch("/c")]
    public virtual bool StopAfter { get; init; } = true;

    [BooleanCommandSwitch("/u")]
    public virtual bool Unicode { get; init; }

    [BooleanCommandSwitch("/a")]
    public virtual bool Ansi { get; init; }

    [BooleanCommandSwitch("/d")]
    public virtual bool DisableAutoRunCommands { get; init; }
}