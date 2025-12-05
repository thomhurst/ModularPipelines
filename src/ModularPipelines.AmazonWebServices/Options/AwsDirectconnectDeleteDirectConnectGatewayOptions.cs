using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "delete-direct-connect-gateway")]
public record AwsDirectconnectDeleteDirectConnectGatewayOptions(
[property: CliOption("--direct-connect-gateway-id")] string DirectConnectGatewayId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}