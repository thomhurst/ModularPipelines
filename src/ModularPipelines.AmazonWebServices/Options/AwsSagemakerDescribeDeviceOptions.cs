using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "describe-device")]
public record AwsSagemakerDescribeDeviceOptions(
[property: CommandSwitch("--device-name")] string DeviceName,
[property: CommandSwitch("--device-fleet-name")] string DeviceFleetName
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}