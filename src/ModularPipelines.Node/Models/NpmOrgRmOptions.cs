using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("org", "rm", "orgname", "username")]
public record NpmOrgRmOptions
(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string OrgName,
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Username
): NpmOptions
{
    [CommandSwitch("--registry")]
    public Uri? Registry { get; set; }

    [CommandSwitch("--otp")]
    public string? Otp { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [BooleanCommandSwitch("--parseable")]
    public bool? Parseable { get; set; }
}