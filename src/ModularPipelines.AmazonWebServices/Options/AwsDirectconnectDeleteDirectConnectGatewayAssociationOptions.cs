using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "delete-direct-connect-gateway-association")]
public record AwsDirectconnectDeleteDirectConnectGatewayAssociationOptions : AwsOptions
{
    [CommandSwitch("--association-id")]
    public string? AssociationId { get; set; }

    [CommandSwitch("--direct-connect-gateway-id")]
    public string? DirectConnectGatewayId { get; set; }

    [CommandSwitch("--virtual-gateway-id")]
    public string? VirtualGatewayId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}