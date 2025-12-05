using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "create-load-balancer-policy")]
public record AwsElbCreateLoadBalancerPolicyOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--policy-type-name")] string PolicyTypeName
) : AwsOptions
{
    [CliOption("--policy-attributes")]
    public string[]? PolicyAttributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}