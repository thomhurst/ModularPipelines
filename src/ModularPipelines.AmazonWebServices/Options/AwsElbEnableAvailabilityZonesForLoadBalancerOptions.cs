using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "enable-availability-zones-for-load-balancer")]
public record AwsElbEnableAvailabilityZonesForLoadBalancerOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--availability-zones")] string[] AvailabilityZones
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}