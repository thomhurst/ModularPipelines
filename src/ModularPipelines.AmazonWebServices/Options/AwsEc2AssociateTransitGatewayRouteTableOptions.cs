using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "associate-transit-gateway-route-table")]
public record AwsEc2AssociateTransitGatewayRouteTableOptions(
[property: CliOption("--transit-gateway-route-table-id")] string TransitGatewayRouteTableId,
[property: CliOption("--transit-gateway-attachment-id")] string TransitGatewayAttachmentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}