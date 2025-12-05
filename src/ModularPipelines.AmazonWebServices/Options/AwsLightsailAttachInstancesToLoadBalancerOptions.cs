using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "attach-instances-to-load-balancer")]
public record AwsLightsailAttachInstancesToLoadBalancerOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--instance-names")] string[] InstanceNames
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}