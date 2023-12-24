using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elb", "set-load-balancer-policies-of-listener")]
public record AwsElbSetLoadBalancerPoliciesOfListenerOptions(
[property: CommandSwitch("--load-balancer-name")] string LoadBalancerName,
[property: CommandSwitch("--load-balancer-port")] int LoadBalancerPort,
[property: CommandSwitch("--policy-names")] string[] PolicyNames
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}