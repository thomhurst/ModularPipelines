using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-monitoring-schedule")]
public record AwsSagemakerCreateMonitoringScheduleOptions(
[property: CliOption("--monitoring-schedule-name")] string MonitoringScheduleName,
[property: CliOption("--monitoring-schedule-config")] string MonitoringScheduleConfig
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}