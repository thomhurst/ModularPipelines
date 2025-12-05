using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-transit-gateway-connect-peer")]
public record AwsEc2CreateTransitGatewayConnectPeerOptions(
[property: CliOption("--transit-gateway-attachment-id")] string TransitGatewayAttachmentId,
[property: CliOption("--peer-address")] string PeerAddress,
[property: CliOption("--inside-cidr-blocks")] string[] InsideCidrBlocks
) : AwsOptions
{
    [CliOption("--transit-gateway-address")]
    public string? TransitGatewayAddress { get; set; }

    [CliOption("--bgp-options")]
    public string? BgpOptions { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}