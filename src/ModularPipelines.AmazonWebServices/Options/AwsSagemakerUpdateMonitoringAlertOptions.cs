using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-monitoring-alert")]
public record AwsSagemakerUpdateMonitoringAlertOptions(
[property: CliOption("--monitoring-schedule-name")] string MonitoringScheduleName,
[property: CliOption("--monitoring-alert-name")] string MonitoringAlertName,
[property: CliOption("--datapoints-to-alert")] int DatapointsToAlert,
[property: CliOption("--evaluation-period")] int EvaluationPeriod
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}