using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "modify-load-balancer-attributes")]
public record AwsElbModifyLoadBalancerAttributesOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--load-balancer-attributes")] string LoadBalancerAttributes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}