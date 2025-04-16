using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("team", "create")]
public record NpmTeamCreateOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Scope,
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Otpcode
) : NpmOptions
{
    [CommandSwitch("--registry")]
    public virtual Uri? Registry { get; set; }

    [CommandSwitch("--otp")]
    public virtual string? Otp { get; set; }

    [BooleanCommandSwitch("--parseable")]
    public virtual bool? Parseable { get; set; }

    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }
}