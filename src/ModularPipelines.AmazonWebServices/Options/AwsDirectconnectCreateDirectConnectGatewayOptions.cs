using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "create-direct-connect-gateway")]
public record AwsDirectconnectCreateDirectConnectGatewayOptions(
[property: CliOption("--direct-connect-gateway-name")] string DirectConnectGatewayName
) : AwsOptions
{
    [CliOption("--amazon-side-asn")]
    public long? AmazonSideAsn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}