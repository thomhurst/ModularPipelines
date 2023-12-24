using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "create-instance")]
public record AwsOpsworksCreateInstanceOptions(
[property: CommandSwitch("--stack-id")] string StackId,
[property: CommandSwitch("--layer-ids")] string[] LayerIds,
[property: CommandSwitch("--instance-type")] string InstanceType
) : AwsOptions
{
    [CommandSwitch("--auto-scaling-type")]
    public string? AutoScalingType { get; set; }

    [CommandSwitch("--hostname")]
    public string? Hostname { get; set; }

    [CommandSwitch("--os")]
    public string? Os { get; set; }

    [CommandSwitch("--ami-id")]
    public string? AmiId { get; set; }

    [CommandSwitch("--ssh-key-name")]
    public string? SshKeyName { get; set; }

    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--virtualization-type")]
    public string? VirtualizationType { get; set; }

    [CommandSwitch("--subnet-id")]
    public string? SubnetId { get; set; }

    [CommandSwitch("--architecture")]
    public string? Architecture { get; set; }

    [CommandSwitch("--root-device-type")]
    public string? RootDeviceType { get; set; }

    [CommandSwitch("--block-device-mappings")]
    public string[]? BlockDeviceMappings { get; set; }

    [CommandSwitch("--agent-version")]
    public string? AgentVersion { get; set; }

    [CommandSwitch("--tenancy")]
    public string? Tenancy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}