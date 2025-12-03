using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "create-direct-connect-gateway-association")]
public record AwsDirectconnectCreateDirectConnectGatewayAssociationOptions(
[property: CliOption("--direct-connect-gateway-id")] string DirectConnectGatewayId
) : AwsOptions
{
    [CliOption("--gateway-id")]
    public string? GatewayId { get; set; }

    [CliOption("--add-allowed-prefixes-to-direct-connect-gateway")]
    public string[]? AddAllowedPrefixesToDirectConnectGateway { get; set; }

    [CliOption("--virtual-gateway-id")]
    public string? VirtualGatewayId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}