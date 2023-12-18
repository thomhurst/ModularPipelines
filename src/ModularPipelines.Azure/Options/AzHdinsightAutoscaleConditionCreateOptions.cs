using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight", "autoscale", "condition", "create")]
public record AzHdinsightAutoscaleConditionCreateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--days")] int Days,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--time")] string Time,
[property: CommandSwitch("--workernode-count")] int WorkernodeCount
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}