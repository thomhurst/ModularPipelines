using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "disassociate-transit-gateway-connect-peer")]
public record AwsNetworkmanagerDisassociateTransitGatewayConnectPeerOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--transit-gateway-connect-peer-arn")] string TransitGatewayConnectPeerArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}