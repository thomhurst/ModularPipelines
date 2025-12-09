using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "create-nodegroup")]
public record AwsEksCreateNodegroupOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--nodegroup-name")] string NodegroupName,
[property: CliOption("--subnets")] string[] Subnets,
[property: CliOption("--node-role")] string NodeRole
) : AwsOptions
{
    [CliOption("--scaling-config")]
    public string? ScalingConfig { get; set; }

    [CliOption("--disk-size")]
    public int? DiskSize { get; set; }

    [CliOption("--instance-types")]
    public string[]? InstanceTypes { get; set; }

    [CliOption("--ami-type")]
    public string? AmiType { get; set; }

    [CliOption("--remote-access")]
    public string? RemoteAccess { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--taints")]
    public string[]? Taints { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--launch-template")]
    public string? LaunchTemplate { get; set; }

    [CliOption("--update-config")]
    public string? UpdateConfig { get; set; }

    [CliOption("--capacity-type")]
    public string? CapacityType { get; set; }

    [CliOption("--release-version")]
    public string? ReleaseVersion { get; set; }

    [CliOption("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}