using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "create-direct-connect-gateway-association-proposal")]
public record AwsDirectconnectCreateDirectConnectGatewayAssociationProposalOptions(
[property: CommandSwitch("--direct-connect-gateway-id")] string DirectConnectGatewayId,
[property: CommandSwitch("--direct-connect-gateway-owner-account")] string DirectConnectGatewayOwnerAccount,
[property: CommandSwitch("--gateway-id")] string GatewayId
) : AwsOptions
{
    [CommandSwitch("--add-allowed-prefixes-to-direct-connect-gateway")]
    public string[]? AddAllowedPrefixesToDirectConnectGateway { get; set; }

    [CommandSwitch("--remove-allowed-prefixes-to-direct-connect-gateway")]
    public string[]? RemoveAllowedPrefixesToDirectConnectGateway { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}