using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "configure-health-check")]
public record AwsElbConfigureHealthCheckOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--health-check")] string HealthCheck
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}