using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-local-gateway-route-table-virtual-interface-group-association")]
public record AwsEc2CreateLocalGatewayRouteTableVirtualInterfaceGroupAssociationOptions(
[property: CommandSwitch("--local-gateway-route-table-id")] string LocalGatewayRouteTableId,
[property: CommandSwitch("--local-gateway-virtual-interface-group-id")] string LocalGatewayVirtualInterfaceGroupId
) : AwsOptions
{
    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}