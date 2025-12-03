using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "vmware", "node-pools", "create")]
public record GcloudContainerVmwareNodePoolsCreateOptions(
[property: CliArgument] string NodePool,
[property: CliArgument] string Cluster,
[property: CliArgument] string Location,
[property: CliOption("--image-type")] string ImageType,
[property: CliOption("--boot-disk-size")] string BootDiskSize,
[property: CliOption("--cpus")] string Cpus,
[property: CliFlag("--enable-load-balancer")] bool EnableLoadBalancer,
[property: CliOption("--image")] string Image,
[property: CliOption("--memory")] string Memory,
[property: CliOption("--node-labels")] IEnumerable<KeyValue> NodeLabels,
[property: CliOption("--node-taints")] IEnumerable<KeyValue> NodeTaints,
[property: CliOption("--replicas")] string Replicas
) : GcloudOptions
{
    [CliOption("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--max-replicas")]
    public string? MaxReplicas { get; set; }

    [CliOption("--min-replicas")]
    public string? MinReplicas { get; set; }
}