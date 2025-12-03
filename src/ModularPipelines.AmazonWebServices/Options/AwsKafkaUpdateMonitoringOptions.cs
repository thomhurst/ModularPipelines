using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "update-monitoring")]
public record AwsKafkaUpdateMonitoringOptions(
[property: CliOption("--cluster-arn")] string ClusterArn,
[property: CliOption("--current-version")] string CurrentVersion
) : AwsOptions
{
    [CliOption("--enhanced-monitoring")]
    public string? EnhancedMonitoring { get; set; }

    [CliOption("--open-monitoring")]
    public string? OpenMonitoring { get; set; }

    [CliOption("--logging-info")]
    public string? LoggingInfo { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}