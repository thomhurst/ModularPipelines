using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "create-load-balancer-listeners")]
public record AwsElbCreateLoadBalancerListenersOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--listeners")] string[] Listeners
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}