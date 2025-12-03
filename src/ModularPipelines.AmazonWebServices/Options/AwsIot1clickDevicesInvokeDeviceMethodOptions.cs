using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot1click-devices", "invoke-device-method")]
public record AwsIot1clickDevicesInvokeDeviceMethodOptions(
[property: CliOption("--device-id")] string DeviceId
) : AwsOptions
{
    [CliOption("--device-method")]
    public string? DeviceMethod { get; set; }

    [CliOption("--device-method-parameters")]
    public string? DeviceMethodParameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}