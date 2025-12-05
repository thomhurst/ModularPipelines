using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "detach-load-balancer-from-subnets")]
public record AwsElbDetachLoadBalancerFromSubnetsOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--subnets")] string[] Subnets
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}