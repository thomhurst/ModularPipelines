using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-transit-gateway-connect-peer")]
public record AwsEc2CreateTransitGatewayConnectPeerOptions(
[property: CommandSwitch("--transit-gateway-attachment-id")] string TransitGatewayAttachmentId,
[property: CommandSwitch("--peer-address")] string PeerAddress,
[property: CommandSwitch("--inside-cidr-blocks")] string[] InsideCidrBlocks
) : AwsOptions
{
    [CommandSwitch("--transit-gateway-address")]
    public string? TransitGatewayAddress { get; set; }

    [CommandSwitch("--bgp-options")]
    public string? BgpOptions { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}