using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-local-gateway-route-table-vpc-association")]
public record AwsEc2CreateLocalGatewayRouteTableVpcAssociationOptions(
[property: CliOption("--local-gateway-route-table-id")] string LocalGatewayRouteTableId,
[property: CliOption("--vpc-id")] string VpcId
) : AwsOptions
{
    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}