using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "modify-load-balancer-attributes")]
public record AwsElbv2ModifyLoadBalancerAttributesOptions(
[property: CliOption("--load-balancer-arn")] string LoadBalancerArn,
[property: CliOption("--attributes")] string[] Attributes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}