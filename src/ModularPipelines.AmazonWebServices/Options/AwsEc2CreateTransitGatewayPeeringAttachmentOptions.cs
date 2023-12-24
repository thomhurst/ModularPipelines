using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-transit-gateway-peering-attachment")]
public record AwsEc2CreateTransitGatewayPeeringAttachmentOptions(
[property: CommandSwitch("--transit-gateway-id")] string TransitGatewayId,
[property: CommandSwitch("--peer-transit-gateway-id")] string PeerTransitGatewayId,
[property: CommandSwitch("--peer-account-id")] string PeerAccountId,
[property: CommandSwitch("--peer-region")] string PeerRegion
) : AwsOptions
{
    [CommandSwitch("--options")]
    public string? Options { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}