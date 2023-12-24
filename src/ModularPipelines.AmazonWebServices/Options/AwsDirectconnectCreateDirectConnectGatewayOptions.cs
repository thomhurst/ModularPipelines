using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "create-direct-connect-gateway")]
public record AwsDirectconnectCreateDirectConnectGatewayOptions(
[property: CommandSwitch("--direct-connect-gateway-name")] string DirectConnectGatewayName
) : AwsOptions
{
    [CommandSwitch("--amazon-side-asn")]
    public long? AmazonSideAsn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}