using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "replace-transit-gateway-route")]
public record AwsEc2ReplaceTransitGatewayRouteOptions(
[property: CliOption("--destination-cidr-block")] string DestinationCidrBlock,
[property: CliOption("--transit-gateway-route-table-id")] string TransitGatewayRouteTableId
) : AwsOptions
{
    [CliOption("--transit-gateway-attachment-id")]
    public string? TransitGatewayAttachmentId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}