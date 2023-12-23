using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "describe-load-balancer-attributes")]
public record AwsElbv2DescribeLoadBalancerAttributesOptions(
[property: CommandSwitch("--load-balancer-arn")] string LoadBalancerArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}