using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "alert-processing-rule", "create")]
public record AzMonitorAlertProcessingRuleCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule-type")] string RuleType,
[property: CommandSwitch("--scopes")] string Scopes
) : AzOptions
{
    [CommandSwitch("--action-groups")]
    public string? ActionGroups { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--filter-alert-context")]
    public string? FilterAlertContext { get; set; }

    [CommandSwitch("--filter-alert-rule-description")]
    public string? FilterAlertRuleDescription { get; set; }

    [CommandSwitch("--filter-alert-rule-id")]
    public string? FilterAlertRuleId { get; set; }

    [CommandSwitch("--filter-alert-rule-name")]
    public string? FilterAlertRuleName { get; set; }

    [CommandSwitch("--filter-monitor-condition")]
    public string? FilterMonitorCondition { get; set; }

    [CommandSwitch("--filter-monitor-service")]
    public string? FilterMonitorService { get; set; }

    [CommandSwitch("--filter-resource-group")]
    public string? FilterResourceGroup { get; set; }

    [CommandSwitch("--filter-resource-type")]
    public string? FilterResourceType { get; set; }

    [CommandSwitch("--filter-severity")]
    public string? FilterSeverity { get; set; }

    [CommandSwitch("--filter-signal-type")]
    public string? FilterSignalType { get; set; }

    [CommandSwitch("--filter-target-resource")]
    public string? FilterTargetResource { get; set; }

    [CommandSwitch("--schedule-end-datetime")]
    public string? ScheduleEndDatetime { get; set; }

    [CommandSwitch("--schedule-recurrence")]
    public string? ScheduleRecurrence { get; set; }

    [CommandSwitch("--schedule-recurrence-2")]
    public string? ScheduleRecurrence2 { get; set; }

    [CommandSwitch("--schedule-recurrence-2-end-time")]
    public string? ScheduleRecurrence2EndTime { get; set; }

    [CommandSwitch("--schedule-recurrence-2-start-time")]
    public string? ScheduleRecurrence2StartTime { get; set; }

    [CommandSwitch("--schedule-recurrence-2-type")]
    public string? ScheduleRecurrence2Type { get; set; }

    [CommandSwitch("--schedule-recurrence-end-time")]
    public string? ScheduleRecurrenceEndTime { get; set; }

    [CommandSwitch("--schedule-recurrence-start-time")]
    public string? ScheduleRecurrenceStartTime { get; set; }

    [CommandSwitch("--schedule-recurrence-type")]
    public string? ScheduleRecurrenceType { get; set; }

    [CommandSwitch("--schedule-start-datetime")]
    public string? ScheduleStartDatetime { get; set; }

    [CommandSwitch("--schedule-time-zone")]
    public string? ScheduleTimeZone { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

