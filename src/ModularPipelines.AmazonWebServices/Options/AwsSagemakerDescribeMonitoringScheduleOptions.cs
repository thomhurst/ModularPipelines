using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "describe-monitoring-schedule")]
public record AwsSagemakerDescribeMonitoringScheduleOptions(
[property: CliOption("--monitoring-schedule-name")] string MonitoringScheduleName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}