using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "disable--network-performance-metric-subscription")]
public record AwsEc2DisableAwsNetworkPerformanceMetricSubscriptionOptions : AwsOptions
{
    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [CommandSwitch("--metric")]
    public string? Metric { get; set; }

    [CommandSwitch("--statistic")]
    public string? Statistic { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}