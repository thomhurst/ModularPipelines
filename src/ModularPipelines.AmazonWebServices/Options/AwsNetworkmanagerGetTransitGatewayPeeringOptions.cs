using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "get-transit-gateway-peering")]
public record AwsNetworkmanagerGetTransitGatewayPeeringOptions(
[property: CommandSwitch("--peering-id")] string PeeringId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}