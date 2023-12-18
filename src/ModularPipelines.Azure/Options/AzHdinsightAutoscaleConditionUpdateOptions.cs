using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight", "autoscale", "condition", "update")]
public record AzHdinsightAutoscaleConditionUpdateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--index")] string Index,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--days")]
    public int? Days { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--time")]
    public string? Time { get; set; }

    [CommandSwitch("--workernode-count")]
    public int? WorkernodeCount { get; set; }
}