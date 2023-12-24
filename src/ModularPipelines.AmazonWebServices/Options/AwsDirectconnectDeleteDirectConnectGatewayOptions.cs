using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "delete-direct-connect-gateway")]
public record AwsDirectconnectDeleteDirectConnectGatewayOptions(
[property: CommandSwitch("--direct-connect-gateway-id")] string DirectConnectGatewayId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}