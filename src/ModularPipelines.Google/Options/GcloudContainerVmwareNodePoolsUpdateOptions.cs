using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "vmware", "node-pools", "update")]
public record GcloudContainerVmwareNodePoolsUpdateOptions(
[property: CliArgument] string NodePool,
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--boot-disk-size")]
    public string? BootDiskSize { get; set; }

    [CliOption("--cpus")]
    public string? Cpus { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--image-type")]
    public string? ImageType { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliOption("--node-labels")]
    public IEnumerable<KeyValue>? NodeLabels { get; set; }

    [CliOption("--node-taints")]
    public IEnumerable<KeyValue>? NodeTaints { get; set; }

    [CliOption("--replicas")]
    public string? Replicas { get; set; }

    [CliFlag("--disable-load-balancer")]
    public bool? DisableLoadBalancer { get; set; }

    [CliFlag("--enable-load-balancer")]
    public bool? EnableLoadBalancer { get; set; }

    [CliOption("--max-replicas")]
    public string? MaxReplicas { get; set; }

    [CliOption("--min-replicas")]
    public string? MinReplicas { get; set; }
}