using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "create-load-balancer")]
public record AwsElbCreateLoadBalancerOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName,
[property: CliOption("--listeners")] string[] Listeners
) : AwsOptions
{
    [CliOption("--availability-zones")]
    public string[]? AvailabilityZones { get; set; }

    [CliOption("--subnets")]
    public string[]? Subnets { get; set; }

    [CliOption("--security-groups")]
    public string[]? SecurityGroups { get; set; }

    [CliOption("--scheme")]
    public string? Scheme { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}