using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hdinsight", "autoscale", "condition", "delete")]
public record AzHdinsightAutoscaleConditionDeleteOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--index")] string Index,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}