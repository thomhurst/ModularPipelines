using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkcloud", "cluster", "metricsconfiguration", "create")]
public record AzNetworkcloudClusterMetricsconfigurationCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--collection-interval")] string CollectionInterval,
[property: CliOption("--extended-location")] string ExtendedLocation,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--enabled-metrics")]
    public bool? EnabledMetrics { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}