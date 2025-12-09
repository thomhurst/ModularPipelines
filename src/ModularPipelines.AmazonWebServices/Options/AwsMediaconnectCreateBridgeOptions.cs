using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "create-bridge")]
public record AwsMediaconnectCreateBridgeOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--placement-arn")] string PlacementArn,
[property: CliOption("--sources")] string[] Sources
) : AwsOptions
{
    [CliOption("--egress-gateway-bridge")]
    public string? EgressGatewayBridge { get; set; }

    [CliOption("--ingress-gateway-bridge")]
    public string? IngressGatewayBridge { get; set; }

    [CliOption("--outputs")]
    public string[]? Outputs { get; set; }

    [CliOption("--source-failover-config")]
    public string? SourceFailoverConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}