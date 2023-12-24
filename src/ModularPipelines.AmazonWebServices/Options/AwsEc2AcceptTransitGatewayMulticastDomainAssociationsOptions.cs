using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "accept-transit-gateway-multicast-domain-associations")]
public record AwsEc2AcceptTransitGatewayMulticastDomainAssociationsOptions : AwsOptions
{
    [CommandSwitch("--transit-gateway-multicast-domain-id")]
    public string? TransitGatewayMulticastDomainId { get; set; }

    [CommandSwitch("--transit-gateway-attachment-id")]
    public string? TransitGatewayAttachmentId { get; set; }

    [CommandSwitch("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}