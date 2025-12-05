using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-local-gateway-route-table-vpc-association")]
public record AwsEc2DeleteLocalGatewayRouteTableVpcAssociationOptions(
[property: CliOption("--local-gateway-route-table-vpc-association-id")] string LocalGatewayRouteTableVpcAssociationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}