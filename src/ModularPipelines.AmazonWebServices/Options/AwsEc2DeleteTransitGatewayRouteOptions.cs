using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-transit-gateway-route")]
public record AwsEc2DeleteTransitGatewayRouteOptions(
[property: CommandSwitch("--transit-gateway-route-table-id")] string TransitGatewayRouteTableId,
[property: CommandSwitch("--destination-cidr-block")] string DestinationCidrBlock
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}