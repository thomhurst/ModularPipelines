using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "attach-load-balancer-to-subnets")]
public record AwsElbAttachLoadBalancerToSubnetsOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--subnets")] string[] Subnets
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}