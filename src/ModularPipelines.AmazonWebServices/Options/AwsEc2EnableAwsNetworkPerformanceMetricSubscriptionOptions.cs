using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "enable--network-performance-metric-subscription")]
public record AwsEc2EnableAwsNetworkPerformanceMetricSubscriptionOptions : AwsOptions
{
    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--metric")]
    public string? Metric { get; set; }

    [CliOption("--statistic")]
    public string? Statistic { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}