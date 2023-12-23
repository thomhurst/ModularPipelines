using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-transit-gateway-connect-peer")]
public record AwsEc2DeleteTransitGatewayConnectPeerOptions(
[property: CommandSwitch("--transit-gateway-connect-peer-id")] string TransitGatewayConnectPeerId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}