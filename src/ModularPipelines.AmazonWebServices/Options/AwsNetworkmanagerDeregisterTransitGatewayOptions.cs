using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "deregister-transit-gateway")]
public record AwsNetworkmanagerDeregisterTransitGatewayOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--transit-gateway-arn")] string TransitGatewayArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}