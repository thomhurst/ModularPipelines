using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managed-cassandra", "cluster", "status")]
public record AzManagedCassandraClusterStatusOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--authentication-method")]
    public string? AuthenticationMethod { get; set; }

    [CommandSwitch("--cassandra-version")]
    public string? CassandraVersion { get; set; }

    [CommandSwitch("--client-certificates")]
    public string? ClientCertificates { get; set; }

    [CommandSwitch("--external-gossip-certificates")]
    public string? ExternalGossipCertificates { get; set; }

    [CommandSwitch("--external-seed-nodes")]
    public string? ExternalSeedNodes { get; set; }

    [CommandSwitch("--hours-between-backups")]
    public string? HoursBetweenBackups { get; set; }

    [CommandSwitch("--identity-type")]
    public string? IdentityType { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--repair-enabled")]
    public bool? RepairEnabled { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}