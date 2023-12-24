using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("snow-device-management", "describe-device")]
public record AwsSnowDeviceManagementDescribeDeviceOptions(
[property: CommandSwitch("--managed-device-id")] string ManagedDeviceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}