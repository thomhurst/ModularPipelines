using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "create-direct-connect-gateway-association-proposal")]
public record AwsDirectconnectCreateDirectConnectGatewayAssociationProposalOptions(
[property: CliOption("--direct-connect-gateway-id")] string DirectConnectGatewayId,
[property: CliOption("--direct-connect-gateway-owner-account")] string DirectConnectGatewayOwnerAccount,
[property: CliOption("--gateway-id")] string GatewayId
) : AwsOptions
{
    [CliOption("--add-allowed-prefixes-to-direct-connect-gateway")]
    public string[]? AddAllowedPrefixesToDirectConnectGateway { get; set; }

    [CliOption("--remove-allowed-prefixes-to-direct-connect-gateway")]
    public string[]? RemoveAllowedPrefixesToDirectConnectGateway { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}