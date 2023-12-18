using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "postgres", "cluster", "create")]
public record AzCosmosdbPostgresClusterCreateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--administrator-login-password")]
    public string? AdministratorLoginPassword { get; set; }

    [CommandSwitch("--citus-version")]
    public string? CitusVersion { get; set; }

    [BooleanCommandSwitch("--coord-public-ip-access")]
    public bool? CoordPublicIpAccess { get; set; }

    [CommandSwitch("--coord-server-edition")]
    public string? CoordServerEdition { get; set; }

    [CommandSwitch("--coordinator-storage")]
    public string? CoordinatorStorage { get; set; }

    [CommandSwitch("--coordinator-v-cores")]
    public string? CoordinatorVCores { get; set; }

    [BooleanCommandSwitch("--enable-ha")]
    public bool? EnableHa { get; set; }

    [BooleanCommandSwitch("--enable-shards-on-coord")]
    public bool? EnableShardsOnCoord { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--maintenance-window")]
    public string? MaintenanceWindow { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--node-count")]
    public int? NodeCount { get; set; }

    [BooleanCommandSwitch("--node-enable-public-ip-access")]
    public bool? NodeEnablePublicIpAccess { get; set; }

    [CommandSwitch("--node-server-edition")]
    public string? NodeServerEdition { get; set; }

    [CommandSwitch("--node-storage")]
    public string? NodeStorage { get; set; }

    [CommandSwitch("--node-v-cores")]
    public string? NodeVCores { get; set; }

    [CommandSwitch("--point-in-time-utc")]
    public string? PointInTimeUtc { get; set; }

    [CommandSwitch("--postgresql-version")]
    public string? PostgresqlVersion { get; set; }

    [CommandSwitch("--preferred-primary-zone")]
    public string? PreferredPrimaryZone { get; set; }

    [CommandSwitch("--source-location")]
    public string? SourceLocation { get; set; }

    [CommandSwitch("--source-resource-id")]
    public string? SourceResourceId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}