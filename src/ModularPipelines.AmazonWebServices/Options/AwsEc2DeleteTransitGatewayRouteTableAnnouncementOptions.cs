using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-transit-gateway-route-table-announcement")]
public record AwsEc2DeleteTransitGatewayRouteTableAnnouncementOptions(
[property: CommandSwitch("--transit-gateway-route-table-announcement-id")] string TransitGatewayRouteTableAnnouncementId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}