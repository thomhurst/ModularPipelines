using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "modify-load-balancer-attributes")]
public record AwsElbv2ModifyLoadBalancerAttributesOptions(
[property: CommandSwitch("--load-balancer-arn")] string LoadBalancerArn,
[property: CommandSwitch("--attributes")] string[] Attributes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}