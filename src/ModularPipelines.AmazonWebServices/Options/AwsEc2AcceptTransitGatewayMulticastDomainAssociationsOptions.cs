using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "accept-transit-gateway-multicast-domain-associations")]
public record AwsEc2AcceptTransitGatewayMulticastDomainAssociationsOptions : AwsOptions
{
    [CliOption("--transit-gateway-multicast-domain-id")]
    public string? TransitGatewayMulticastDomainId { get; set; }

    [CliOption("--transit-gateway-attachment-id")]
    public string? TransitGatewayAttachmentId { get; set; }

    [CliOption("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}