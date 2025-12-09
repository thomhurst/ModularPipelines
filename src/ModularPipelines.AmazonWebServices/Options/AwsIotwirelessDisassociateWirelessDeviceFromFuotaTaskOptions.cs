using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "disassociate-wireless-device-from-fuota-task")]
public record AwsIotwirelessDisassociateWirelessDeviceFromFuotaTaskOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--wireless-device-id")] string WirelessDeviceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}