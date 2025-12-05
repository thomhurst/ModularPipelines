using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hdinsight", "autoscale", "create")]
public record AzHdinsightAutoscaleCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliOption("--days")]
    public int? Days { get; set; }

    [CliOption("--max-workernode-count")]
    public int? MaxWorkernodeCount { get; set; }

    [CliOption("--min-workernode-count")]
    public int? MinWorkernodeCount { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--time")]
    public string? Time { get; set; }

    [CliOption("--timezone")]
    public string? Timezone { get; set; }

    [CliOption("--workernode-count")]
    public int? WorkernodeCount { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}