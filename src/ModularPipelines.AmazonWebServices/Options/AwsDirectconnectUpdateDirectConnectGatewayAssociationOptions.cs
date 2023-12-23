using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "update-direct-connect-gateway-association")]
public record AwsDirectconnectUpdateDirectConnectGatewayAssociationOptions : AwsOptions
{
    [CommandSwitch("--association-id")]
    public string? AssociationId { get; set; }

    [CommandSwitch("--add-allowed-prefixes-to-direct-connect-gateway")]
    public string[]? AddAllowedPrefixesToDirectConnectGateway { get; set; }

    [CommandSwitch("--remove-allowed-prefixes-to-direct-connect-gateway")]
    public string[]? RemoveAllowedPrefixesToDirectConnectGateway { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}