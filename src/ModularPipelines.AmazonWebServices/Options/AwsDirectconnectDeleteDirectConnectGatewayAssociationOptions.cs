using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "delete-direct-connect-gateway-association")]
public record AwsDirectconnectDeleteDirectConnectGatewayAssociationOptions : AwsOptions
{
    [CliOption("--association-id")]
    public string? AssociationId { get; set; }

    [CliOption("--direct-connect-gateway-id")]
    public string? DirectConnectGatewayId { get; set; }

    [CliOption("--virtual-gateway-id")]
    public string? VirtualGatewayId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}