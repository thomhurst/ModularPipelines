using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "job-schedule", "create")]
public record AzBatchJobScheduleCreateOptions : AzOptions
{
    [CommandSwitch("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [BooleanCommandSwitch("--allow-task-preemption")]
    public bool? AllowTaskPreemption { get; set; }

    [BooleanCommandSwitch("--do-not-run-after")]
    public bool? DoNotRunAfter { get; set; }

    [BooleanCommandSwitch("--do-not-run-until")]
    public bool? DoNotRunUntil { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--job-manager-task-command-line")]
    public string? JobManagerTaskCommandLine { get; set; }

    [CommandSwitch("--job-manager-task-id")]
    public string? JobManagerTaskId { get; set; }

    [CommandSwitch("--job-manager-task-resource-files")]
    public string? JobManagerTaskResourceFiles { get; set; }

    [CommandSwitch("--job-max-task-retry-count")]
    public int? JobMaxTaskRetryCount { get; set; }

    [CommandSwitch("--job-max-wall-clock-time")]
    public string? JobMaxWallClockTime { get; set; }

    [CommandSwitch("--json-file")]
    public string? JsonFile { get; set; }

    [CommandSwitch("--max-parallel-tasks")]
    public string? MaxParallelTasks { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--on-all-tasks-complete")]
    public string? OnAllTasksComplete { get; set; }

    [CommandSwitch("--pool-id")]
    public string? PoolId { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--recurrence-interval")]
    public string? RecurrenceInterval { get; set; }

    [CommandSwitch("--required-slots")]
    public string? RequiredSlots { get; set; }

    [CommandSwitch("--start-window")]
    public string? StartWindow { get; set; }

    [CommandSwitch("--uses-task-dependencies")]
    public string? UsesTaskDependencies { get; set; }
}