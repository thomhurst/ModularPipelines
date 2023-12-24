using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "disassociate-transit-gateway-route-table")]
public record AwsEc2DisassociateTransitGatewayRouteTableOptions(
[property: CommandSwitch("--transit-gateway-route-table-id")] string TransitGatewayRouteTableId,
[property: CommandSwitch("--transit-gateway-attachment-id")] string TransitGatewayAttachmentId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}