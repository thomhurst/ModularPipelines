using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "register-devices")]
public record AwsSagemakerRegisterDevicesOptions(
[property: CliOption("--device-fleet-name")] string DeviceFleetName,
[property: CliOption("--devices")] string[] Devices
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}