using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "create-bridge")]
public record AwsMediaconnectCreateBridgeOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--placement-arn")] string PlacementArn,
[property: CommandSwitch("--sources")] string[] Sources
) : AwsOptions
{
    [CommandSwitch("--egress-gateway-bridge")]
    public string? EgressGatewayBridge { get; set; }

    [CommandSwitch("--ingress-gateway-bridge")]
    public string? IngressGatewayBridge { get; set; }

    [CommandSwitch("--outputs")]
    public string[]? Outputs { get; set; }

    [CommandSwitch("--source-failover-config")]
    public string? SourceFailoverConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}