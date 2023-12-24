using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elb", "describe-load-balancer-policies")]
public record AwsElbDescribeLoadBalancerPoliciesOptions : AwsOptions
{
    [CommandSwitch("--load-balancer-name")]
    public string? LoadBalancerName { get; set; }

    [CommandSwitch("--policy-names")]
    public string[]? PolicyNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}