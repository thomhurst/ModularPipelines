using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "create-transit-gateway-peering")]
public record AwsNetworkmanagerCreateTransitGatewayPeeringOptions(
[property: CliOption("--core-network-id")] string CoreNetworkId,
[property: CliOption("--transit-gateway-arn")] string TransitGatewayArn
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}