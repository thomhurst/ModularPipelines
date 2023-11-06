using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "set", "registry")]
public record NpmConfigSetRegistryOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Registry
) : NpmOptions
{
    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--editor")]
    public string? Editor { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--long")]
    public bool? Long { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Key { get; set; }
}