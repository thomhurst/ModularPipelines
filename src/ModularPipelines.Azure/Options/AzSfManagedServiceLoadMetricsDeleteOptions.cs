using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "managed-service", "load-metrics", "delete")]
public record AzSfManagedServiceLoadMetricsDeleteOptions(
[property: CliOption("--application")] string Application,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;