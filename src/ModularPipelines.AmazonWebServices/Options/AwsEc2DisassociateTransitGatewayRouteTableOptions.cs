using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "disassociate-transit-gateway-route-table")]
public record AwsEc2DisassociateTransitGatewayRouteTableOptions(
[property: CliOption("--transit-gateway-route-table-id")] string TransitGatewayRouteTableId,
[property: CliOption("--transit-gateway-attachment-id")] string TransitGatewayAttachmentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}