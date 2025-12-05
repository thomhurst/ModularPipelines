using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elb", "describe-instance-health")]
public record AwsElbDescribeInstanceHealthOptions(
[property: CliOption("--load-balancer-name")] string LoadBalancerName
) : AwsOptions
{
    [CliOption("--instances")]
    public string[]? Instances { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}