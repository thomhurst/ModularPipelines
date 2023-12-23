using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafka", "update-monitoring")]
public record AwsKafkaUpdateMonitoringOptions(
[property: CommandSwitch("--cluster-arn")] string ClusterArn,
[property: CommandSwitch("--current-version")] string CurrentVersion
) : AwsOptions
{
    [CommandSwitch("--enhanced-monitoring")]
    public string? EnhancedMonitoring { get; set; }

    [CommandSwitch("--open-monitoring")]
    public string? OpenMonitoring { get; set; }

    [CommandSwitch("--logging-info")]
    public string? LoggingInfo { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}