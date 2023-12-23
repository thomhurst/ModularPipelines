using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("snow-device-management", "describe-device-ec2-instances")]
public record AwsSnowDeviceManagementDescribeDeviceEc2InstancesOptions(
[property: CommandSwitch("--instance-ids")] string[] InstanceIds,
[property: CommandSwitch("--managed-device-id")] string ManagedDeviceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}