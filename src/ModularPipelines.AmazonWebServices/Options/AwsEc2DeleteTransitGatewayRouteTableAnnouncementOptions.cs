using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-transit-gateway-route-table-announcement")]
public record AwsEc2DeleteTransitGatewayRouteTableAnnouncementOptions(
[property: CliOption("--transit-gateway-route-table-announcement-id")] string TransitGatewayRouteTableAnnouncementId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}