using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "clusters", "update")]
public record GcloudBigtableClustersUpdateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Instance,
[property: CommandSwitch("--autoscaling-cpu-target")] string AutoscalingCpuTarget,
[property: CommandSwitch("--autoscaling-max-nodes")] string AutoscalingMaxNodes,
[property: CommandSwitch("--autoscaling-min-nodes")] string AutoscalingMinNodes,
[property: CommandSwitch("--autoscaling-storage-target")] string AutoscalingStorageTarget,
[property: CommandSwitch("--num-nodes")] string NumNodes,
[property: BooleanCommandSwitch("--disable-autoscaling")] bool DisableAutoscaling
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}