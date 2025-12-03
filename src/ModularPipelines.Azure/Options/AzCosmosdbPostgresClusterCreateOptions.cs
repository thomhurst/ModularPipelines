using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "postgres", "cluster", "create")]
public record AzCosmosdbPostgresClusterCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--administrator-login-password")]
    public string? AdministratorLoginPassword { get; set; }

    [CliOption("--citus-version")]
    public string? CitusVersion { get; set; }

    [CliFlag("--coord-public-ip-access")]
    public bool? CoordPublicIpAccess { get; set; }

    [CliOption("--coord-server-edition")]
    public string? CoordServerEdition { get; set; }

    [CliOption("--coordinator-storage")]
    public string? CoordinatorStorage { get; set; }

    [CliOption("--coordinator-v-cores")]
    public string? CoordinatorVCores { get; set; }

    [CliFlag("--enable-ha")]
    public bool? EnableHa { get; set; }

    [CliFlag("--enable-shards-on-coord")]
    public bool? EnableShardsOnCoord { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--maintenance-window")]
    public string? MaintenanceWindow { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--node-count")]
    public int? NodeCount { get; set; }

    [CliFlag("--node-enable-public-ip-access")]
    public bool? NodeEnablePublicIpAccess { get; set; }

    [CliOption("--node-server-edition")]
    public string? NodeServerEdition { get; set; }

    [CliOption("--node-storage")]
    public string? NodeStorage { get; set; }

    [CliOption("--node-v-cores")]
    public string? NodeVCores { get; set; }

    [CliOption("--point-in-time-utc")]
    public string? PointInTimeUtc { get; set; }

    [CliOption("--postgresql-version")]
    public string? PostgresqlVersion { get; set; }

    [CliOption("--preferred-primary-zone")]
    public string? PreferredPrimaryZone { get; set; }

    [CliOption("--source-location")]
    public string? SourceLocation { get; set; }

    [CliOption("--source-resource-id")]
    public string? SourceResourceId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}