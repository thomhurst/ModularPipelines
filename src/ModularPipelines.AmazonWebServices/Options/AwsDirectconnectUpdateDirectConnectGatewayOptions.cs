using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "update-direct-connect-gateway")]
public record AwsDirectconnectUpdateDirectConnectGatewayOptions(
[property: CliOption("--direct-connect-gateway-id")] string DirectConnectGatewayId,
[property: CliOption("--new-direct-connect-gateway-name")] string NewDirectConnectGatewayName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}