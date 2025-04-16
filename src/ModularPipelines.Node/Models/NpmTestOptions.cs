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
    public virtual bool? IgnoreScripts { get; set; }

    [CommandSwitch("--script-shell")]
    public virtual string? ScriptShell { get; set; }
}