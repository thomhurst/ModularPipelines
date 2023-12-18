using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "create")]
public record AzGrafanaCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--deterministic-outbound-ip")]
    public string? DeterministicOutboundIp { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--principal-ids")]
    public string? PrincipalIds { get; set; }

    [CommandSwitch("--principal-types")]
    public string? PrincipalTypes { get; set; }

    [BooleanCommandSwitch("--skip-role-assignments")]
    public bool? SkipRoleAssignments { get; set; }

    [BooleanCommandSwitch("--skip-system-assigned-identity")]
    public bool? SkipSystemAssignedIdentity { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--zone-redundancy")]
    public string? ZoneRedundancy { get; set; }
}