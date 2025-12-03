using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("snow-device-management", "describe-device-ec2-instances")]
public record AwsSnowDeviceManagementDescribeDeviceEc2InstancesOptions(
[property: CliOption("--instance-ids")] string[] InstanceIds,
[property: CliOption("--managed-device-id")] string ManagedDeviceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}