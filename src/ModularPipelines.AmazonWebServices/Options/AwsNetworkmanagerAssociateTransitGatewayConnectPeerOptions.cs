using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "associate-transit-gateway-connect-peer")]
public record AwsNetworkmanagerAssociateTransitGatewayConnectPeerOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--transit-gateway-connect-peer-arn")] string TransitGatewayConnectPeerArn,
[property: CliOption("--device-id")] string DeviceId
) : AwsOptions
{
    [CliOption("--link-id")]
    public string? LinkId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}