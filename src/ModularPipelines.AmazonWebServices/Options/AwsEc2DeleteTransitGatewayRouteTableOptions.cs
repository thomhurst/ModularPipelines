using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-transit-gateway-route-table")]
public record AwsEc2DeleteTransitGatewayRouteTableOptions(
[property: CommandSwitch("--transit-gateway-route-table-id")] string TransitGatewayRouteTableId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}