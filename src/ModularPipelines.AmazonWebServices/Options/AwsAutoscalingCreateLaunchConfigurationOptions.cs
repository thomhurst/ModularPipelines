using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "create-launch-configuration")]
public record AwsAutoscalingCreateLaunchConfigurationOptions(
[property: CommandSwitch("--launch-configuration-name")] string LaunchConfigurationName
) : AwsOptions
{
    [CommandSwitch("--image-id")]
    public string? ImageId { get; set; }

    [CommandSwitch("--key-name")]
    public string? KeyName { get; set; }

    [CommandSwitch("--security-groups")]
    public string[]? SecurityGroups { get; set; }

    [CommandSwitch("--classic-link-vpc-id")]
    public string? ClassicLinkVpcId { get; set; }

    [CommandSwitch("--classic-link-vpc-security-groups")]
    public string[]? ClassicLinkVpcSecurityGroups { get; set; }

    [CommandSwitch("--user-data")]
    public string? UserData { get; set; }

    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--kernel-id")]
    public string? KernelId { get; set; }

    [CommandSwitch("--ramdisk-id")]
    public string? RamdiskId { get; set; }

    [CommandSwitch("--block-device-mappings")]
    public string[]? BlockDeviceMappings { get; set; }

    [CommandSwitch("--instance-monitoring")]
    public string? InstanceMonitoring { get; set; }

    [CommandSwitch("--spot-price")]
    public string? SpotPrice { get; set; }

    [CommandSwitch("--iam-instance-profile")]
    public string? IamInstanceProfile { get; set; }

    [CommandSwitch("--placement-tenancy")]
    public string? PlacementTenancy { get; set; }

    [CommandSwitch("--metadata-options")]
    public string? MetadataOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}