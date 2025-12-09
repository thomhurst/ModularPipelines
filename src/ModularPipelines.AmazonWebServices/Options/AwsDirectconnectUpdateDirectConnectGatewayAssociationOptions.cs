using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "update-direct-connect-gateway-association")]
public record AwsDirectconnectUpdateDirectConnectGatewayAssociationOptions : AwsOptions
{
    [CliOption("--association-id")]
    public string? AssociationId { get; set; }

    [CliOption("--add-allowed-prefixes-to-direct-connect-gateway")]
    public string[]? AddAllowedPrefixesToDirectConnectGateway { get; set; }

    [CliOption("--remove-allowed-prefixes-to-direct-connect-gateway")]
    public string[]? RemoveAllowedPrefixesToDirectConnectGateway { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}