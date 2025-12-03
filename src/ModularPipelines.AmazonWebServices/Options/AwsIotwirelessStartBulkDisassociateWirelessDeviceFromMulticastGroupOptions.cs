using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "start-bulk-disassociate-wireless-device-from-multicast-group")]
public record AwsIotwirelessStartBulkDisassociateWirelessDeviceFromMulticastGroupOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--query-string")]
    public string? QueryString { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}