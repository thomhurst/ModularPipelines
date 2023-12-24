using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "create-direct-connect-gateway-association")]
public record AwsDirectconnectCreateDirectConnectGatewayAssociationOptions(
[property: CommandSwitch("--direct-connect-gateway-id")] string DirectConnectGatewayId
) : AwsOptions
{
    [CommandSwitch("--gateway-id")]
    public string? GatewayId { get; set; }

    [CommandSwitch("--add-allowed-prefixes-to-direct-connect-gateway")]
    public string[]? AddAllowedPrefixesToDirectConnectGateway { get; set; }

    [CommandSwitch("--virtual-gateway-id")]
    public string? VirtualGatewayId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}