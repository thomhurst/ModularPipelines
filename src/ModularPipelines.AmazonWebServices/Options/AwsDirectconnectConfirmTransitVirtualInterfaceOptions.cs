using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "confirm-transit-virtual-interface")]
public record AwsDirectconnectConfirmTransitVirtualInterfaceOptions(
[property: CommandSwitch("--virtual-interface-id")] string VirtualInterfaceId,
[property: CommandSwitch("--direct-connect-gateway-id")] string DirectConnectGatewayId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}