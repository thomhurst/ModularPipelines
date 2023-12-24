using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "create-transit-gateway-route-table-attachment")]
public record AwsNetworkmanagerCreateTransitGatewayRouteTableAttachmentOptions(
[property: CommandSwitch("--peering-id")] string PeeringId,
[property: CommandSwitch("--transit-gateway-route-table-arn")] string TransitGatewayRouteTableArn
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}