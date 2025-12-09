using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-devices")]
public record AwsSagemakerUpdateDevicesOptions(
[property: CliOption("--device-fleet-name")] string DeviceFleetName,
[property: CliOption("--devices")] string[] Devices
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}