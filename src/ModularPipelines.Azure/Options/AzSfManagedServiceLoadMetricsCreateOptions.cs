using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "managed-service", "load-metrics", "create")]
public record AzSfManagedServiceLoadMetricsCreateOptions(
[property: CliOption("--application")] string Application,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--default-load")]
    public string? DefaultLoad { get; set; }

    [CliOption("--primary-default-load")]
    public string? PrimaryDefaultLoad { get; set; }

    [CliOption("--secondary-default-load")]
    public string? SecondaryDefaultLoad { get; set; }

    [CliOption("--weight")]
    public string? Weight { get; set; }
}