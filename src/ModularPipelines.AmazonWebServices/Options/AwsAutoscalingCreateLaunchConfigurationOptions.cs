using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "create-launch-configuration")]
public record AwsAutoscalingCreateLaunchConfigurationOptions(
[property: CliOption("--launch-configuration-name")] string LaunchConfigurationName
) : AwsOptions
{
    [CliOption("--image-id")]
    public string? ImageId { get; set; }

    [CliOption("--key-name")]
    public string? KeyName { get; set; }

    [CliOption("--security-groups")]
    public string[]? SecurityGroups { get; set; }

    [CliOption("--classic-link-vpc-id")]
    public string? ClassicLinkVpcId { get; set; }

    [CliOption("--classic-link-vpc-security-groups")]
    public string[]? ClassicLinkVpcSecurityGroups { get; set; }

    [CliOption("--user-data")]
    public string? UserData { get; set; }

    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--kernel-id")]
    public string? KernelId { get; set; }

    [CliOption("--ramdisk-id")]
    public string? RamdiskId { get; set; }

    [CliOption("--block-device-mappings")]
    public string[]? BlockDeviceMappings { get; set; }

    [CliOption("--instance-monitoring")]
    public string? InstanceMonitoring { get; set; }

    [CliOption("--spot-price")]
    public string? SpotPrice { get; set; }

    [CliOption("--iam-instance-profile")]
    public string? IamInstanceProfile { get; set; }

    [CliOption("--placement-tenancy")]
    public string? PlacementTenancy { get; set; }

    [CliOption("--metadata-options")]
    public string? MetadataOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}