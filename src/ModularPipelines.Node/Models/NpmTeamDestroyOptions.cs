using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("team", "destroy")]
public record NpmTeamDestroyOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Scope,
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Otpcode
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
}