using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-transit-gateway-route")]
public record AwsEc2DeleteTransitGatewayRouteOptions(
[property: CliOption("--transit-gateway-route-table-id")] string TransitGatewayRouteTableId,
[property: CliOption("--destination-cidr-block")] string DestinationCidrBlock
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}