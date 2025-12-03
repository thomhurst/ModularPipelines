using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hdinsight", "autoscale", "condition", "update")]
public record AzHdinsightAutoscaleConditionUpdateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--index")] string Index,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--days")]
    public int? Days { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--time")]
    public string? Time { get; set; }

    [CliOption("--workernode-count")]
    public int? WorkernodeCount { get; set; }
}