using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "accept-direct-connect-gateway-association-proposal")]
public record AwsDirectconnectAcceptDirectConnectGatewayAssociationProposalOptions(
[property: CliOption("--direct-connect-gateway-id")] string DirectConnectGatewayId,
[property: CliOption("--proposal-id")] string ProposalId,
[property: CliOption("--associated-gateway-owner-account")] string AssociatedGatewayOwnerAccount
) : AwsOptions
{
    [CliOption("--override-allowed-prefixes-to-direct-connect-gateway")]
    public string[]? OverrideAllowedPrefixesToDirectConnectGateway { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}