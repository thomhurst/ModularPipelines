using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "enable-transit-gateway-route-table-propagation")]
public record AwsEc2EnableTransitGatewayRouteTablePropagationOptions(
[property: CliOption("--transit-gateway-route-table-id")] string TransitGatewayRouteTableId
) : AwsOptions
{
    [CliOption("--transit-gateway-attachment-id")]
    public string? TransitGatewayAttachmentId { get; set; }

    [CliOption("--transit-gateway-route-table-announcement-id")]
    public string? TransitGatewayRouteTableAnnouncementId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}