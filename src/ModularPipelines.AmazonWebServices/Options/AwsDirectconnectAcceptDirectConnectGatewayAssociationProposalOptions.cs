using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "accept-direct-connect-gateway-association-proposal")]
public record AwsDirectconnectAcceptDirectConnectGatewayAssociationProposalOptions(
[property: CommandSwitch("--direct-connect-gateway-id")] string DirectConnectGatewayId,
[property: CommandSwitch("--proposal-id")] string ProposalId,
[property: CommandSwitch("--associated-gateway-owner-account")] string AssociatedGatewayOwnerAccount
) : AwsOptions
{
    [CommandSwitch("--override-allowed-prefixes-to-direct-connect-gateway")]
    public string[]? OverrideAllowedPrefixesToDirectConnectGateway { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}