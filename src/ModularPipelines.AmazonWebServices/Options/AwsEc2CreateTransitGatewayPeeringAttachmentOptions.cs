using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-transit-gateway-peering-attachment")]
public record AwsEc2CreateTransitGatewayPeeringAttachmentOptions(
[property: CliOption("--transit-gateway-id")] string TransitGatewayId,
[property: CliOption("--peer-transit-gateway-id")] string PeerTransitGatewayId,
[property: CliOption("--peer-account-id")] string PeerAccountId,
[property: CliOption("--peer-region")] string PeerRegion
) : AwsOptions
{
    [CliOption("--options")]
    public string? Options { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}