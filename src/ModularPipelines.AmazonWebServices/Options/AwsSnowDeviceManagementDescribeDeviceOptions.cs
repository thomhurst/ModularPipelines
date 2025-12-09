using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("snow-device-management", "describe-device")]
public record AwsSnowDeviceManagementDescribeDeviceOptions(
[property: CliOption("--managed-device-id")] string ManagedDeviceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}