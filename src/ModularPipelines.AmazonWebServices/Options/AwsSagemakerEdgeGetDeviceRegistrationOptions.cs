using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker-edge", "get-device-registration")]
public record AwsSagemakerEdgeGetDeviceRegistrationOptions(
[property: CliOption("--device-name")] string DeviceName,
[property: CliOption("--device-fleet-name")] string DeviceFleetName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}