using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-local-gateway-route-table-virtual-interface-group-association")]
public record AwsEc2CreateLocalGatewayRouteTableVirtualInterfaceGroupAssociationOptions(
[property: CliOption("--local-gateway-route-table-id")] string LocalGatewayRouteTableId,
[property: CliOption("--local-gateway-virtual-interface-group-id")] string LocalGatewayVirtualInterfaceGroupId
) : AwsOptions
{
    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}