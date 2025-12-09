using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("outposts", "start-connection")]
public record AwsOutpostsStartConnectionOptions(
[property: CliOption("--device-serial-number")] string DeviceSerialNumber,
[property: CliOption("--asset-id")] string AssetId,
[property: CliOption("--client-public-key")] string ClientPublicKey,
[property: CliOption("--network-interface-device-index")] int NetworkInterfaceDeviceIndex
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}