using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kusto", "cluster", "detach-follower-database")]
public record AzKustoClusterDetachFollowerDatabaseOptions(
[property: CliFlag("--attached-database-configuration-name")] bool AttachedDatabaseConfigurationName,
[property: CliOption("--cluster-resource-id")] string ClusterResourceId
) : AzOptions
{
    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}