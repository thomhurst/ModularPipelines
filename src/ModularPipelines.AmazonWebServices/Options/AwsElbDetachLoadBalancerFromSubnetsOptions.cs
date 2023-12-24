using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elb", "detach-load-balancer-from-subnets")]
public record AwsElbDetachLoadBalancerFromSubnetsOptions(
[property: CommandSwitch("--load-balancer-name")] string LoadBalancerName,
[property: CommandSwitch("--subnets")] string[] Subnets
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}