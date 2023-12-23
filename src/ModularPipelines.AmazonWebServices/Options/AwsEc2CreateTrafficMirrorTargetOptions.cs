using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-traffic-mirror-target")]
public record AwsEc2CreateTrafficMirrorTargetOptions : AwsOptions
{
    [CommandSwitch("--network-interface-id")]
    public string? NetworkInterfaceId { get; set; }

    [CommandSwitch("--network-load-balancer-arn")]
    public string? NetworkLoadBalancerArn { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--gateway-load-balancer-endpoint-id")]
    public string? GatewayLoadBalancerEndpointId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}