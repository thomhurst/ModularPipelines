using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "detach-instances-from-load-balancer")]
public record AwsLightsailDetachInstancesFromLoadBalancerOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--instance-names")] string[] InstanceNames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}