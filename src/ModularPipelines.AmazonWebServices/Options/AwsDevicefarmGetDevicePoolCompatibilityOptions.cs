using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devicefarm", "get-device-pool-compatibility")]
public record AwsDevicefarmGetDevicePoolCompatibilityOptions(
[property: CliOption("--device-pool-arn")] string DevicePoolArn
) : AwsOptions
{
    [CliOption("--app-arn")]
    public string? AppArn { get; set; }

    [CliOption("--test-type")]
    public string? TestType { get; set; }

    [CliOption("--test")]
    public string? Test { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}