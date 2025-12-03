using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "deregister-wireless-device")]
public record AwsIotwirelessDeregisterWirelessDeviceOptions(
[property: CliOption("--identifier")] string Identifier
) : AwsOptions
{
    [CliOption("--wireless-device-type")]
    public string? WirelessDeviceType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}