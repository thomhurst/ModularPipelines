using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-transit-gateway-route")]
public record AwsEc2CreateTransitGatewayRouteOptions(
[property: CommandSwitch("--destination-cidr-block")] string DestinationCidrBlock,
[property: CommandSwitch("--transit-gateway-route-table-id")] string TransitGatewayRouteTableId
) : AwsOptions
{
    [CommandSwitch("--transit-gateway-attachment-id")]
    public string? TransitGatewayAttachmentId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}