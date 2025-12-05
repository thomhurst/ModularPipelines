using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "describe-load-balancer-policy-types")]
public record AwsElbDescribeLoadBalancerPolicyTypesOptions : AwsOptions
{
    [CliOption("--policy-type-names")]
    public string[]? PolicyTypeNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}