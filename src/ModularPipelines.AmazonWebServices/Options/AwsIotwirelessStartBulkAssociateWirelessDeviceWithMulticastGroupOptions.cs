using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "start-bulk-associate-wireless-device-with-multicast-group")]
public record AwsIotwirelessStartBulkAssociateWirelessDeviceWithMulticastGroupOptions(
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