using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hdinsight", "autoscale", "condition", "create")]
public record AzHdinsightAutoscaleConditionCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--days")] int Days,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--time")] string Time,
[property: CliOption("--workernode-count")] int WorkernodeCount
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}