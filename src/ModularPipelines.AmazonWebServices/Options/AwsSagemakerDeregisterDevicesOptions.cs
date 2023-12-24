using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "deregister-devices")]
public record AwsSagemakerDeregisterDevicesOptions(
[property: CommandSwitch("--device-fleet-name")] string DeviceFleetName,
[property: CommandSwitch("--device-names")] string[] DeviceNames
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}