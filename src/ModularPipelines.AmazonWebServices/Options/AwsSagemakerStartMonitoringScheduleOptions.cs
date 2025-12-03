using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "start-monitoring-schedule")]
public record AwsSagemakerStartMonitoringScheduleOptions(
[property: CliOption("--monitoring-schedule-name")] string MonitoringScheduleName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}