using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-transit-gateway-prefix-list-reference")]
public record AwsEc2CreateTransitGatewayPrefixListReferenceOptions(
[property: CliOption("--transit-gateway-route-table-id")] string TransitGatewayRouteTableId,
[property: CliOption("--prefix-list-id")] string PrefixListId
) : AwsOptions
{
    [CliOption("--transit-gateway-attachment-id")]
    public string? TransitGatewayAttachmentId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}