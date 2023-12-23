using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-devices")]
public record AwsSagemakerUpdateDevicesOptions(
[property: CommandSwitch("--device-fleet-name")] string DeviceFleetName,
[property: CommandSwitch("--devices")] string[] Devices
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}