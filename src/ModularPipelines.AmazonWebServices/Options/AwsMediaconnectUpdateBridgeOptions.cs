using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "update-bridge")]
public record AwsMediaconnectUpdateBridgeOptions(
[property: CommandSwitch("--bridge-arn")] string BridgeArn
) : AwsOptions
{
    [CommandSwitch("--egress-gateway-bridge")]
    public string? EgressGatewayBridge { get; set; }

    [CommandSwitch("--ingress-gateway-bridge")]
    public string? IngressGatewayBridge { get; set; }

    [CommandSwitch("--source-failover-config")]
    public string? SourceFailoverConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}