using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot1click-devices", "invoke-device-method")]
public record AwsIot1clickDevicesInvokeDeviceMethodOptions(
[property: CommandSwitch("--device-id")] string DeviceId
) : AwsOptions
{
    [CommandSwitch("--device-method")]
    public string? DeviceMethod { get; set; }

    [CommandSwitch("--device-method-parameters")]
    public string? DeviceMethodParameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}