using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-monitoring-schedule")]
public record AwsSagemakerUpdateMonitoringScheduleOptions(
[property: CommandSwitch("--monitoring-schedule-name")] string MonitoringScheduleName,
[property: CommandSwitch("--monitoring-schedule-config")] string MonitoringScheduleConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}