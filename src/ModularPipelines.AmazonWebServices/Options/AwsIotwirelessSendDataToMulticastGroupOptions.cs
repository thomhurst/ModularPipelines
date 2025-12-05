using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "send-data-to-multicast-group")]
public record AwsIotwirelessSendDataToMulticastGroupOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--payload-data")] string PayloadData,
[property: CliOption("--wireless-metadata")] string WirelessMetadata
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}