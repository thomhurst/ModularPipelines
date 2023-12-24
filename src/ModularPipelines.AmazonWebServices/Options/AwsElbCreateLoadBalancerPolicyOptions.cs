using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elb", "create-load-balancer-policy")]
public record AwsElbCreateLoadBalancerPolicyOptions(
[property: CommandSwitch("--load-balancer-name")] string LoadBalancerName,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--policy-type-name")] string PolicyTypeName
) : AwsOptions
{
    [CommandSwitch("--policy-attributes")]
    public string[]? PolicyAttributes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}