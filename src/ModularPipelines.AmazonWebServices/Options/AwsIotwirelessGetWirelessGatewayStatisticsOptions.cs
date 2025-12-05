using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "get-wireless-gateway-statistics")]
public record AwsIotwirelessGetWirelessGatewayStatisticsOptions(
[property: CliOption("--wireless-gateway-id")] string WirelessGatewayId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}