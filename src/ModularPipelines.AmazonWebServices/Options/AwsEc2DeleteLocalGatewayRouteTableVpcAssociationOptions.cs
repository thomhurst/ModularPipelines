using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-local-gateway-route-table-vpc-association")]
public record AwsEc2DeleteLocalGatewayRouteTableVpcAssociationOptions(
[property: CommandSwitch("--local-gateway-route-table-vpc-association-id")] string LocalGatewayRouteTableVpcAssociationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}