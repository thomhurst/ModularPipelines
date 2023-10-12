using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("test")]
public record NpmTestOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Args
) : NpmOptions
{
    [BooleanCommandSwitch("--ignore-scripts")]
    public bool? IgnoreScripts { get; set; }

    [CommandSwitch("--script-shell")]
    public string? ScriptShell { get; set; }
}