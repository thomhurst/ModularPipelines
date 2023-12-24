using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "enable-transit-gateway-route-table-propagation")]
public record AwsEc2EnableTransitGatewayRouteTablePropagationOptions(
[property: CommandSwitch("--transit-gateway-route-table-id")] string TransitGatewayRouteTableId
) : AwsOptions
{
    [CommandSwitch("--transit-gateway-attachment-id")]
    public string? TransitGatewayAttachmentId { get; set; }

    [CommandSwitch("--transit-gateway-route-table-announcement-id")]
    public string? TransitGatewayRouteTableAnnouncementId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}