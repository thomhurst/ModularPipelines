using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "start-monitoring-schedule")]
public record AwsSagemakerStartMonitoringScheduleOptions(
[property: CommandSwitch("--monitoring-schedule-name")] string MonitoringScheduleName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}