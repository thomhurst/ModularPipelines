using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "detach-elastic-load-balancer")]
public record AwsOpsworksDetachElasticLoadBalancerOptions(
[property: CliOption("--elastic-load-balancer-name")] string ElasticLoadBalancerName,
[property: CliOption("--layer-id")] string LayerId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}