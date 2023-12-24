using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "register-image")]
public record AwsEc2RegisterImageOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--image-location")]
    public string? ImageLocation { get; set; }

    [CommandSwitch("--architecture")]
    public string? Architecture { get; set; }

    [CommandSwitch("--block-device-mappings")]
    public string[]? BlockDeviceMappings { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--kernel-id")]
    public string? KernelId { get; set; }

    [CommandSwitch("--billing-products")]
    public string[]? BillingProducts { get; set; }

    [CommandSwitch("--ramdisk-id")]
    public string? RamdiskId { get; set; }

    [CommandSwitch("--root-device-name")]
    public string? RootDeviceName { get; set; }

    [CommandSwitch("--sriov-net-support")]
    public string? SriovNetSupport { get; set; }

    [CommandSwitch("--virtualization-type")]
    public string? VirtualizationType { get; set; }

    [CommandSwitch("--boot-mode")]
    public string? BootMode { get; set; }

    [CommandSwitch("--tpm-support")]
    public string? TpmSupport { get; set; }

    [CommandSwitch("--uefi-data")]
    public string? UefiData { get; set; }

    [CommandSwitch("--imds-support")]
    public string? ImdsSupport { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}