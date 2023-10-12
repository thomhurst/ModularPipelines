using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("org", "set")]
public record NpmOrgSetOptions
(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string OrgName,
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Username
): NpmOptions
{
    [PositionalArgument(Position = Position.BeforeSwitches)] 
    public string PermissionLevel { get; set; }
    
    [CommandSwitch("--registry")]
    public Uri? Registry { get; set; }

    [CommandSwitch("--otp")]
    public string? Otp { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [BooleanCommandSwitch("--parseable")]
    public bool? Parseable { get; set; }
}