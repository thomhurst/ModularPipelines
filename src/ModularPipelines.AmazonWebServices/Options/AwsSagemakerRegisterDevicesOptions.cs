using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "register-devices")]
public record AwsSagemakerRegisterDevicesOptions(
[property: CommandSwitch("--device-fleet-name")] string DeviceFleetName,
[property: CommandSwitch("--devices")] string[] Devices
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}