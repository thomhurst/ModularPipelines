using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-monitoring-schedule")]
public record AwsSagemakerCreateMonitoringScheduleOptions(
[property: CommandSwitch("--monitoring-schedule-name")] string MonitoringScheduleName,
[property: CommandSwitch("--monitoring-schedule-config")] string MonitoringScheduleConfig
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}