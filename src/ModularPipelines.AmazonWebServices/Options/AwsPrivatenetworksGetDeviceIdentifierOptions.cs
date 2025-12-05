using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privatenetworks", "get-device-identifier")]
public record AwsPrivatenetworksGetDeviceIdentifierOptions(
[property: CliOption("--device-identifier-arn")] string DeviceIdentifierArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}