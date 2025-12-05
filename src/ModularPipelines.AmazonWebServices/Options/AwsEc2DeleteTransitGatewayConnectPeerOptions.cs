using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-transit-gateway-connect-peer")]
public record AwsEc2DeleteTransitGatewayConnectPeerOptions(
[property: CliOption("--transit-gateway-connect-peer-id")] string TransitGatewayConnectPeerId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}