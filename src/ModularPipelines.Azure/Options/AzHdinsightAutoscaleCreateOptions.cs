using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight", "autoscale", "create")]
public record AzHdinsightAutoscaleCreateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [CommandSwitch("--days")]
    public int? Days { get; set; }

    [CommandSwitch("--max-workernode-count")]
    public int? MaxWorkernodeCount { get; set; }

    [CommandSwitch("--min-workernode-count")]
    public int? MinWorkernodeCount { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--time")]
    public string? Time { get; set; }

    [CommandSwitch("--timezone")]
    public string? Timezone { get; set; }

    [CommandSwitch("--workernode-count")]
    public int? WorkernodeCount { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

