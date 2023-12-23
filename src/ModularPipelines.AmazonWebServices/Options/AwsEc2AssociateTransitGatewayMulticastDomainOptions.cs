using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "associate-transit-gateway-multicast-domain")]
public record AwsEc2AssociateTransitGatewayMulticastDomainOptions(
[property: CommandSwitch("--transit-gateway-multicast-domain-id")] string TransitGatewayMulticastDomainId,
[property: CommandSwitch("--transit-gateway-attachment-id")] string TransitGatewayAttachmentId,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}