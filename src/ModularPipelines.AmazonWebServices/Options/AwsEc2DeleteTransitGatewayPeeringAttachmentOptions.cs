using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-transit-gateway-peering-attachment")]
public record AwsEc2DeleteTransitGatewayPeeringAttachmentOptions(
[property: CommandSwitch("--transit-gateway-attachment-id")] string TransitGatewayAttachmentId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}