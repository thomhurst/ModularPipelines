using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "get-wireless-gateway-statistics")]
public record AwsIotwirelessGetWirelessGatewayStatisticsOptions(
[property: CommandSwitch("--wireless-gateway-id")] string WirelessGatewayId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}