using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "backup-policy", "trigger", "create-schedule")]
public record AzDataprotectionBackupPolicyTriggerCreateScheduleOptions(
[property: CliOption("--interval-count")] int IntervalCount,
[property: CliOption("--interval-type")] int IntervalType,
[property: CliOption("--schedule-days")] string ScheduleDays
) : AzOptions;