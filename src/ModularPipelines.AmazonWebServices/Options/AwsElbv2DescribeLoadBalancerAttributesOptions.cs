using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "describe-load-balancer-attributes")]
public record AwsElbv2DescribeLoadBalancerAttributesOptions(
[property: CliOption("--load-balancer-arn")] string LoadBalancerArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}