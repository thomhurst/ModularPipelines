using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-local-gateway-route-table")]
public record AwsEc2DeleteLocalGatewayRouteTableOptions(
[property: CommandSwitch("--local-gateway-route-table-id")] string LocalGatewayRouteTableId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}