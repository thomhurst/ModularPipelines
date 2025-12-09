using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "register-image")]
public record AwsEc2RegisterImageOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--image-location")]
    public string? ImageLocation { get; set; }

    [CliOption("--architecture")]
    public string? Architecture { get; set; }

    [CliOption("--block-device-mappings")]
    public string[]? BlockDeviceMappings { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--kernel-id")]
    public string? KernelId { get; set; }

    [CliOption("--billing-products")]
    public string[]? BillingProducts { get; set; }

    [CliOption("--ramdisk-id")]
    public string? RamdiskId { get; set; }

    [CliOption("--root-device-name")]
    public string? RootDeviceName { get; set; }

    [CliOption("--sriov-net-support")]
    public string? SriovNetSupport { get; set; }

    [CliOption("--virtualization-type")]
    public string? VirtualizationType { get; set; }

    [CliOption("--boot-mode")]
    public string? BootMode { get; set; }

    [CliOption("--tpm-support")]
    public string? TpmSupport { get; set; }

    [CliOption("--uefi-data")]
    public string? UefiData { get; set; }

    [CliOption("--imds-support")]
    public string? ImdsSupport { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}