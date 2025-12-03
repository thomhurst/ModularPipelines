using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managed-cassandra", "cluster", "update")]
public record AzManagedCassandraClusterUpdateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--authentication-method")]
    public string? AuthenticationMethod { get; set; }

    [CliOption("--cassandra-version")]
    public string? CassandraVersion { get; set; }

    [CliOption("--client-certificates")]
    public string? ClientCertificates { get; set; }

    [CliOption("--external-gossip-certificates")]
    public string? ExternalGossipCertificates { get; set; }

    [CliOption("--external-seed-nodes")]
    public string? ExternalSeedNodes { get; set; }

    [CliOption("--hours-between-backups")]
    public string? HoursBetweenBackups { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--repair-enabled")]
    public bool? RepairEnabled { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}