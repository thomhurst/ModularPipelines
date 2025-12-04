using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("grafana", "create")]
public record AzGrafanaCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--deterministic-outbound-ip")]
    public string? DeterministicOutboundIp { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--principal-ids")]
    public string? PrincipalIds { get; set; }

    [CliOption("--principal-types")]
    public string? PrincipalTypes { get; set; }

    [CliFlag("--skip-role-assignments")]
    public bool? SkipRoleAssignments { get; set; }

    [CliFlag("--skip-system-assigned-identity")]
    public bool? SkipSystemAssignedIdentity { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--zone-redundancy")]
    public string? ZoneRedundancy { get; set; }
}