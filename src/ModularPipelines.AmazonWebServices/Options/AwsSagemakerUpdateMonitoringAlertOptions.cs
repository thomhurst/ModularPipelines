using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-monitoring-alert")]
public record AwsSagemakerUpdateMonitoringAlertOptions(
[property: CommandSwitch("--monitoring-schedule-name")] string MonitoringScheduleName,
[property: CommandSwitch("--monitoring-alert-name")] string MonitoringAlertName,
[property: CommandSwitch("--datapoints-to-alert")] int DatapointsToAlert,
[property: CommandSwitch("--evaluation-period")] int EvaluationPeriod
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}