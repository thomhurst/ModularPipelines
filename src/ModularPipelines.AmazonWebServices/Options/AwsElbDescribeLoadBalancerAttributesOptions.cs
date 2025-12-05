using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "describe-load-balancer-attributes")]
public record AwsElbDescribeLoadBalancerAttributesOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}