using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "associate-transit-gateway-multicast-domain")]
public record AwsEc2AssociateTransitGatewayMulticastDomainOptions(
[property: CliOption("--transit-gateway-multicast-domain-id")] string TransitGatewayMulticastDomainId,
[property: CliOption("--transit-gateway-attachment-id")] string TransitGatewayAttachmentId,
[property: CliOption("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}