using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "delete-load-balancer-listeners")]
public record AwsElbDeleteLoadBalancerListenersOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--load-balancer-ports")] string[] LoadBalancerPorts
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}