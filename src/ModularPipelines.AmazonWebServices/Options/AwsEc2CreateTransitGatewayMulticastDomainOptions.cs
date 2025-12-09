using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-transit-gateway-multicast-domain")]
public record AwsEc2CreateTransitGatewayMulticastDomainOptions(
[property: CliOption("--transit-gateway-id")] string TransitGatewayId
) : AwsOptions
{
    [CliOption("--options")]
    public string? Options { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}