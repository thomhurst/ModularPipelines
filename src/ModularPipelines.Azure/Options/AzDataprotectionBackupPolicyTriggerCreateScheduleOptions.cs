using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-policy", "trigger", "create-schedule")]
public record AzDataprotectionBackupPolicyTriggerCreateScheduleOptions(
[property: CommandSwitch("--interval-count")] int IntervalCount,
[property: CommandSwitch("--interval-type")] int IntervalType,
[property: CommandSwitch("--schedule-days")] string ScheduleDays
) : AzOptions
{
}

