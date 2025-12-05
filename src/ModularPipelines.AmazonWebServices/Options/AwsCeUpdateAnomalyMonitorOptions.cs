using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "update-anomaly-monitor")]
public record AwsCeUpdateAnomalyMonitorOptions(
[property: CliOption("--monitor-arn")] string MonitorArn
) : AwsOptions
{
    [CliOption("--monitor-name")]
    public string? MonitorName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}