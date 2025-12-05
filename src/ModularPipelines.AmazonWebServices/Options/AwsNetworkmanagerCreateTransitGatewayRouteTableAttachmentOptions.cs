using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "create-transit-gateway-route-table-attachment")]
public record AwsNetworkmanagerCreateTransitGatewayRouteTableAttachmentOptions(
[property: CliOption("--peering-id")] string PeeringId,
[property: CliOption("--transit-gateway-route-table-arn")] string TransitGatewayRouteTableArn
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}