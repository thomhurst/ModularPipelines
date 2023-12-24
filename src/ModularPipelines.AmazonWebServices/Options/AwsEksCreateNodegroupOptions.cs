using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "create-nodegroup")]
public record AwsEksCreateNodegroupOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--nodegroup-name")] string NodegroupName,
[property: CommandSwitch("--subnets")] string[] Subnets,
[property: CommandSwitch("--node-role")] string NodeRole
) : AwsOptions
{
    [CommandSwitch("--scaling-config")]
    public string? ScalingConfig { get; set; }

    [CommandSwitch("--disk-size")]
    public int? DiskSize { get; set; }

    [CommandSwitch("--instance-types")]
    public string[]? InstanceTypes { get; set; }

    [CommandSwitch("--ami-type")]
    public string? AmiType { get; set; }

    [CommandSwitch("--remote-access")]
    public string? RemoteAccess { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--taints")]
    public string[]? Taints { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--launch-template")]
    public string? LaunchTemplate { get; set; }

    [CommandSwitch("--update-config")]
    public string? UpdateConfig { get; set; }

    [CommandSwitch("--capacity-type")]
    public string? CapacityType { get; set; }

    [CommandSwitch("--release-version")]
    public string? ReleaseVersion { get; set; }

    [CommandSwitch("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}