using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "create-transit-gateway-peering")]
public record AwsNetworkmanagerCreateTransitGatewayPeeringOptions(
[property: CommandSwitch("--core-network-id")] string CoreNetworkId,
[property: CommandSwitch("--transit-gateway-arn")] string TransitGatewayArn
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}