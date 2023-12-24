using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elb", "deregister-instances-from-load-balancer")]
public record AwsElbDeregisterInstancesFromLoadBalancerOptions(
[property: CommandSwitch("--load-balancer-name")] string LoadBalancerName,
[property: CommandSwitch("--instances")] string[] Instances
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}