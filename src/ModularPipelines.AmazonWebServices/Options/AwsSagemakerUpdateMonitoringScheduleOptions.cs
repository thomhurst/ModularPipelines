using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-monitoring-schedule")]
public record AwsSagemakerUpdateMonitoringScheduleOptions(
[property: CliOption("--monitoring-schedule-name")] string MonitoringScheduleName,
[property: CliOption("--monitoring-schedule-config")] string MonitoringScheduleConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}