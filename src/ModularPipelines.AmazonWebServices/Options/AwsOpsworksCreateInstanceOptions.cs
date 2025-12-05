using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "create-instance")]
public record AwsOpsworksCreateInstanceOptions(
[property: CliOption("--stack-id")] string StackId,
[property: CliOption("--layer-ids")] string[] LayerIds,
[property: CliOption("--instance-type")] string InstanceType
) : AwsOptions
{
    [CliOption("--auto-scaling-type")]
    public string? AutoScalingType { get; set; }

    [CliOption("--hostname")]
    public string? Hostname { get; set; }

    [CliOption("--os")]
    public string? Os { get; set; }

    [CliOption("--ami-id")]
    public string? AmiId { get; set; }

    [CliOption("--ssh-key-name")]
    public string? SshKeyName { get; set; }

    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--virtualization-type")]
    public string? VirtualizationType { get; set; }

    [CliOption("--subnet-id")]
    public string? SubnetId { get; set; }

    [CliOption("--architecture")]
    public string? Architecture { get; set; }

    [CliOption("--root-device-type")]
    public string? RootDeviceType { get; set; }

    [CliOption("--block-device-mappings")]
    public string[]? BlockDeviceMappings { get; set; }

    [CliOption("--agent-version")]
    public string? AgentVersion { get; set; }

    [CliOption("--tenancy")]
    public string? Tenancy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}