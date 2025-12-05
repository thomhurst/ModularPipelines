using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "deregister-devices")]
public record AwsSagemakerDeregisterDevicesOptions(
[property: CliOption("--device-fleet-name")] string DeviceFleetName,
[property: CliOption("--device-names")] string[] DeviceNames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}