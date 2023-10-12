using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pkg", "set")]
public record NpmPkgSetOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Value
) : NpmOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [CommandSwitch("--workspace")]
    public string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public bool? Workspaces { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Key { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Array { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Index { get; set; }
}