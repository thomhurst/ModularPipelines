using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("org", "ls", "orgname")]
public record NpmOrgLsOptions
(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string OrgName
) : NpmOptions
{
    [CommandSwitch("--registry")]
    public virtual Uri? Registry { get; set; }

    [CommandSwitch("--otp")]
    public virtual string? Otp { get; set; }

    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [BooleanCommandSwitch("--parseable")]
    public virtual bool? Parseable { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Username { get; set; }
}