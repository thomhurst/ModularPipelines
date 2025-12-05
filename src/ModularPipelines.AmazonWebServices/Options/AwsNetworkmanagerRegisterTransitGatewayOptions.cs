using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "register-transit-gateway")]
public record AwsNetworkmanagerRegisterTransitGatewayOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--transit-gateway-arn")] string TransitGatewayArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}