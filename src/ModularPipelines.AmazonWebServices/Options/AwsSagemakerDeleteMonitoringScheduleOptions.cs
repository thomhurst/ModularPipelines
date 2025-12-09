using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "delete-monitoring-schedule")]
public record AwsSagemakerDeleteMonitoringScheduleOptions(
[property: CliOption("--monitoring-schedule-name")] string MonitoringScheduleName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}