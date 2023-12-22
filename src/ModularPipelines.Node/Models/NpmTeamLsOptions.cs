using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("team", "ls")]
public record NpmTeamLsOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Value
) : NpmOptions
{
    [CommandSwitch("--registry")]
    public Uri? Registry { get; set; }

    [CommandSwitch("--otp")]
    public string? Otp { get; set; }

    [BooleanCommandSwitch("--parseable")]
    public bool? Parseable { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Scope { get; set; }
}