using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "vmware", "node-pools", "create")]
public record GcloudContainerVmwareNodePoolsCreateOptions(
[property: PositionalArgument] string NodePool,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--image-type")] string ImageType,
[property: CommandSwitch("--boot-disk-size")] string BootDiskSize,
[property: CommandSwitch("--cpus")] string Cpus,
[property: BooleanCommandSwitch("--enable-load-balancer")] bool EnableLoadBalancer,
[property: CommandSwitch("--image")] string Image,
[property: CommandSwitch("--memory")] string Memory,
[property: CommandSwitch("--node-labels")] IEnumerable<KeyValue> NodeLabels,
[property: CommandSwitch("--node-taints")] IEnumerable<KeyValue> NodeTaints,
[property: CommandSwitch("--replicas")] string Replicas
) : GcloudOptions
{
    [CommandSwitch("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--max-replicas")]
    public string? MaxReplicas { get; set; }

    [CommandSwitch("--min-replicas")]
    public string? MinReplicas { get; set; }
}