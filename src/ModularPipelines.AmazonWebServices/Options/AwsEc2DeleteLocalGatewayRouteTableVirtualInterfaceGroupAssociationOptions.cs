using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-local-gateway-route-table-virtual-interface-group-association")]
public record AwsEc2DeleteLocalGatewayRouteTableVirtualInterfaceGroupAssociationOptions(
[property: CliOption("--local-gateway-route-table-virtual-interface-group-association-id")] string LocalGatewayRouteTableVirtualInterfaceGroupAssociationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}