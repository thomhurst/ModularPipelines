using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "confirm-transit-virtual-interface")]
public record AwsDirectconnectConfirmTransitVirtualInterfaceOptions(
[property: CliOption("--virtual-interface-id")] string VirtualInterfaceId,
[property: CliOption("--direct-connect-gateway-id")] string DirectConnectGatewayId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}