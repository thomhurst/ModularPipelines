using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-instance-attribute")]
public record AwsEc2ModifyInstanceAttributeOptions(
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--attribute")]
    public string? Attribute { get; set; }

    [CliOption("--block-device-mappings")]
    public string[]? BlockDeviceMappings { get; set; }

    [CliOption("--groups")]
    public string[]? Groups { get; set; }

    [CliOption("--instance-initiated-shutdown-behavior")]
    public string? InstanceInitiatedShutdownBehavior { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--kernel")]
    public string? Kernel { get; set; }

    [CliOption("--ramdisk")]
    public string? Ramdisk { get; set; }

    [CliOption("--sriov-net-support")]
    public string? SriovNetSupport { get; set; }

    [CliOption("--user-data")]
    public string? UserData { get; set; }

    [CliOption("--value")]
    public string? Value { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}