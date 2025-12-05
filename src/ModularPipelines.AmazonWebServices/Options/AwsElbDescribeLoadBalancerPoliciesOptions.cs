using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "describe-load-balancer-policies")]
public record AwsElbDescribeLoadBalancerPoliciesOptions : AwsOptions
{
    [CliOption("--load-balancer-name")]
    public string? LoadBalancerName { get; set; }

    [CliOption("--policy-names")]
    public string[]? PolicyNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}