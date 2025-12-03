using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-traffic-mirror-target")]
public record AwsEc2CreateTrafficMirrorTargetOptions : AwsOptions
{
    [CliOption("--network-interface-id")]
    public string? NetworkInterfaceId { get; set; }

    [CliOption("--network-load-balancer-arn")]
    public string? NetworkLoadBalancerArn { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--gateway-load-balancer-endpoint-id")]
    public string? GatewayLoadBalancerEndpointId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}