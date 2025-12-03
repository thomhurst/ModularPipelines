using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hdinsight", "autoscale", "update")]
public record AzHdinsightAutoscaleUpdateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--max-workernode-count")]
    public int? MaxWorkernodeCount { get; set; }

    [CliOption("--min-workernode-count")]
    public int? MinWorkernodeCount { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--timezone")]
    public string? Timezone { get; set; }
}