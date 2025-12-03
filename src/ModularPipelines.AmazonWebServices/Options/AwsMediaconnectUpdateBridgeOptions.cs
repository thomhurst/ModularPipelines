using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "update-bridge")]
public record AwsMediaconnectUpdateBridgeOptions(
[property: CliOption("--bridge-arn")] string BridgeArn
) : AwsOptions
{
    [CliOption("--egress-gateway-bridge")]
    public string? EgressGatewayBridge { get; set; }

    [CliOption("--ingress-gateway-bridge")]
    public string? IngressGatewayBridge { get; set; }

    [CliOption("--source-failover-config")]
    public string? SourceFailoverConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}