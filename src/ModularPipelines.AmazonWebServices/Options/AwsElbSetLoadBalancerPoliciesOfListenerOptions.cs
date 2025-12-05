using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "set-load-balancer-policies-of-listener")]
public record AwsElbSetLoadBalancerPoliciesOfListenerOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--load-balancer-port")] int LoadBalancerPort,
[property: CliOption("--policy-names")] string[] PolicyNames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}