using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "vmware", "node-pools", "update")]
public record GcloudContainerVmwareNodePoolsUpdateOptions(
[property: PositionalArgument] string NodePool,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--boot-disk-size")]
    public string? BootDiskSize { get; set; }

    [CommandSwitch("--cpus")]
    public string? Cpus { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--image-type")]
    public string? ImageType { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [CommandSwitch("--node-labels")]
    public IEnumerable<KeyValue>? NodeLabels { get; set; }

    [CommandSwitch("--node-taints")]
    public IEnumerable<KeyValue>? NodeTaints { get; set; }

    [CommandSwitch("--replicas")]
    public string? Replicas { get; set; }

    [BooleanCommandSwitch("--disable-load-balancer")]
    public bool? DisableLoadBalancer { get; set; }

    [BooleanCommandSwitch("--enable-load-balancer")]
    public bool? EnableLoadBalancer { get; set; }

    [CommandSwitch("--max-replicas")]
    public string? MaxReplicas { get; set; }

    [CommandSwitch("--min-replicas")]
    public string? MinReplicas { get; set; }
}