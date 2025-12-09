using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-transit-gateway-multicast-domain")]
public record AwsEc2DeleteTransitGatewayMulticastDomainOptions(
[property: CliOption("--transit-gateway-multicast-domain-id")] string TransitGatewayMulticastDomainId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}