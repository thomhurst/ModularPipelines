using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fleet", "member", "create")]
public record AzFleetMemberCreateOptions(
[property: CommandSwitch("--fleet-name")] string FleetName,
[property: CommandSwitch("--member-cluster-id")] string MemberClusterId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--update-group")]
    public string? UpdateGroup { get; set; }
}