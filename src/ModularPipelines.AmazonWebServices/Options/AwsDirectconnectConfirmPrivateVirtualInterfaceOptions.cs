using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "confirm-private-virtual-interface")]
public record AwsDirectconnectConfirmPrivateVirtualInterfaceOptions(
[property: CliOption("--virtual-interface-id")] string VirtualInterfaceId
) : AwsOptions
{
    [CliOption("--virtual-gateway-id")]
    public string? VirtualGatewayId { get; set; }

    [CliOption("--direct-connect-gateway-id")]
    public string? DirectConnectGatewayId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}