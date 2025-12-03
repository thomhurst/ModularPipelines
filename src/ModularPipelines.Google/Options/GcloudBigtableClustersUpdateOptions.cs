using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "clusters", "update")]
public record GcloudBigtableClustersUpdateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Instance,
[property: CliOption("--autoscaling-cpu-target")] string AutoscalingCpuTarget,
[property: CliOption("--autoscaling-max-nodes")] string AutoscalingMaxNodes,
[property: CliOption("--autoscaling-min-nodes")] string AutoscalingMinNodes,
[property: CliOption("--autoscaling-storage-target")] string AutoscalingStorageTarget,
[property: CliOption("--num-nodes")] string NumNodes,
[property: CliFlag("--disable-autoscaling")] bool DisableAutoscaling
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}