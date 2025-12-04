using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "alert-processing-rule", "create")]
public record AzMonitorAlertProcessingRuleCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-type")] string RuleType,
[property: CliOption("--scopes")] string Scopes
) : AzOptions
{
    [CliOption("--action-groups")]
    public string? ActionGroups { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--filter-alert-context")]
    public string? FilterAlertContext { get; set; }

    [CliOption("--filter-alert-rule-description")]
    public string? FilterAlertRuleDescription { get; set; }

    [CliOption("--filter-alert-rule-id")]
    public string? FilterAlertRuleId { get; set; }

    [CliOption("--filter-alert-rule-name")]
    public string? FilterAlertRuleName { get; set; }

    [CliOption("--filter-monitor-condition")]
    public string? FilterMonitorCondition { get; set; }

    [CliOption("--filter-monitor-service")]
    public string? FilterMonitorService { get; set; }

    [CliOption("--filter-resource-group")]
    public string? FilterResourceGroup { get; set; }

    [CliOption("--filter-resource-type")]
    public string? FilterResourceType { get; set; }

    [CliOption("--filter-severity")]
    public string? FilterSeverity { get; set; }

    [CliOption("--filter-signal-type")]
    public string? FilterSignalType { get; set; }

    [CliOption("--filter-target-resource")]
    public string? FilterTargetResource { get; set; }

    [CliOption("--schedule-end-datetime")]
    public string? ScheduleEndDatetime { get; set; }

    [CliOption("--schedule-recurrence")]
    public string? ScheduleRecurrence { get; set; }

    [CliOption("--schedule-recurrence-2")]
    public string? ScheduleRecurrence2 { get; set; }

    [CliOption("--schedule-recurrence-2-end-time")]
    public string? ScheduleRecurrence2EndTime { get; set; }

    [CliOption("--schedule-recurrence-2-start-time")]
    public string? ScheduleRecurrence2StartTime { get; set; }

    [CliOption("--schedule-recurrence-2-type")]
    public string? ScheduleRecurrence2Type { get; set; }

    [CliOption("--schedule-recurrence-end-time")]
    public string? ScheduleRecurrenceEndTime { get; set; }

    [CliOption("--schedule-recurrence-start-time")]
    public string? ScheduleRecurrenceStartTime { get; set; }

    [CliOption("--schedule-recurrence-type")]
    public string? ScheduleRecurrenceType { get; set; }

    [CliOption("--schedule-start-datetime")]
    public string? ScheduleStartDatetime { get; set; }

    [CliOption("--schedule-time-zone")]
    public string? ScheduleTimeZone { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}