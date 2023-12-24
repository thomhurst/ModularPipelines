using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "confirm-private-virtual-interface")]
public record AwsDirectconnectConfirmPrivateVirtualInterfaceOptions(
[property: CommandSwitch("--virtual-interface-id")] string VirtualInterfaceId
) : AwsOptions
{
    [CommandSwitch("--virtual-gateway-id")]
    public string? VirtualGatewayId { get; set; }

    [CommandSwitch("--direct-connect-gateway-id")]
    public string? DirectConnectGatewayId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}