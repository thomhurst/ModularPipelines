using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-local-gateway-route-table-virtual-interface-group-association")]
public record AwsEc2DeleteLocalGatewayRouteTableVirtualInterfaceGroupAssociationOptions(
[property: CommandSwitch("--local-gateway-route-table-virtual-interface-group-association-id")] string LocalGatewayRouteTableVirtualInterfaceGroupAssociationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}