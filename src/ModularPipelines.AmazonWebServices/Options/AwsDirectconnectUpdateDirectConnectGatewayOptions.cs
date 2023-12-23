using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "update-direct-connect-gateway")]
public record AwsDirectconnectUpdateDirectConnectGatewayOptions(
[property: CommandSwitch("--direct-connect-gateway-id")] string DirectConnectGatewayId,
[property: CommandSwitch("--new-direct-connect-gateway-name")] string NewDirectConnectGatewayName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}