using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "postgres", "cluster", "update")]
public record AzCosmosdbPostgresClusterUpdateOptions : AzOptions
{
    [CliOption("--administrator-login-password")]
    public string? AdministratorLoginPassword { get; set; }

    [CliOption("--citus-version")]
    public string? CitusVersion { get; set; }

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

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

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--maintenance-window")]
    public string? MaintenanceWindow { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--node-count")]
    public int? NodeCount { get; set; }

    [CliOption("--node-server-edition")]
    public string? NodeServerEdition { get; set; }

    [CliOption("--node-storage")]
    public string? NodeStorage { get; set; }

    [CliOption("--node-v-cores")]
    public string? NodeVCores { get; set; }

    [CliOption("--postgresql-version")]
    public string? PostgresqlVersion { get; set; }

    [CliOption("--preferred-primary-zone")]
    public string? PreferredPrimaryZone { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}