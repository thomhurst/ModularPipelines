using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "register-instances-with-load-balancer")]
public record AwsElbRegisterInstancesWithLoadBalancerOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--instances")] string[] Instances
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}