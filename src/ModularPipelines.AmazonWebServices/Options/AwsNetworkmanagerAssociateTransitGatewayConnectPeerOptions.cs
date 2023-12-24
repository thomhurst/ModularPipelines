using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "associate-transit-gateway-connect-peer")]
public record AwsNetworkmanagerAssociateTransitGatewayConnectPeerOptions(
[property: CommandSwitch("--global-network-id")] string GlobalNetworkId,
[property: CommandSwitch("--transit-gateway-connect-peer-arn")] string TransitGatewayConnectPeerArn,
[property: CommandSwitch("--device-id")] string DeviceId
) : AwsOptions
{
    [CommandSwitch("--link-id")]
    public string? LinkId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}