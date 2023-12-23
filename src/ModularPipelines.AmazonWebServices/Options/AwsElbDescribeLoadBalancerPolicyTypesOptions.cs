using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elb", "describe-load-balancer-policy-types")]
public record AwsElbDescribeLoadBalancerPolicyTypesOptions : AwsOptions
{
    [CommandSwitch("--policy-type-names")]
    public string[]? PolicyTypeNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}