using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-instance-attribute")]
public record AwsEc2ModifyInstanceAttributeOptions(
[property: CommandSwitch("--instance-id")] string InstanceId
) : AwsOptions
{
    [CommandSwitch("--attribute")]
    public string? Attribute { get; set; }

    [CommandSwitch("--block-device-mappings")]
    public string[]? BlockDeviceMappings { get; set; }

    [CommandSwitch("--groups")]
    public string[]? Groups { get; set; }

    [CommandSwitch("--instance-initiated-shutdown-behavior")]
    public string? InstanceInitiatedShutdownBehavior { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--kernel")]
    public string? Kernel { get; set; }

    [CommandSwitch("--ramdisk")]
    public string? Ramdisk { get; set; }

    [CommandSwitch("--sriov-net-support")]
    public string? SriovNetSupport { get; set; }

    [CommandSwitch("--user-data")]
    public string? UserData { get; set; }

    [CommandSwitch("--value")]
    public string? Value { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}