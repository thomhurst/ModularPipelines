using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-transit-gateway-prefix-list-reference")]
public record AwsEc2CreateTransitGatewayPrefixListReferenceOptions(
[property: CommandSwitch("--transit-gateway-route-table-id")] string TransitGatewayRouteTableId,
[property: CommandSwitch("--prefix-list-id")] string PrefixListId
) : AwsOptions
{
    [CommandSwitch("--transit-gateway-attachment-id")]
    public string? TransitGatewayAttachmentId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}