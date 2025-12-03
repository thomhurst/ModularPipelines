using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "job-schedule", "create")]
public record AzBatchJobScheduleCreateOptions : AzOptions
{
    [CliOption("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliFlag("--allow-task-preemption")]
    public bool? AllowTaskPreemption { get; set; }

    [CliFlag("--do-not-run-after")]
    public bool? DoNotRunAfter { get; set; }

    [CliFlag("--do-not-run-until")]
    public bool? DoNotRunUntil { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--job-manager-task-command-line")]
    public string? JobManagerTaskCommandLine { get; set; }

    [CliOption("--job-manager-task-id")]
    public string? JobManagerTaskId { get; set; }

    [CliOption("--job-manager-task-resource-files")]
    public string? JobManagerTaskResourceFiles { get; set; }

    [CliOption("--job-max-task-retry-count")]
    public int? JobMaxTaskRetryCount { get; set; }

    [CliOption("--job-max-wall-clock-time")]
    public string? JobMaxWallClockTime { get; set; }

    [CliOption("--json-file")]
    public string? JsonFile { get; set; }

    [CliOption("--max-parallel-tasks")]
    public string? MaxParallelTasks { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--on-all-tasks-complete")]
    public string? OnAllTasksComplete { get; set; }

    [CliOption("--pool-id")]
    public string? PoolId { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--recurrence-interval")]
    public string? RecurrenceInterval { get; set; }

    [CliOption("--required-slots")]
    public string? RequiredSlots { get; set; }

    [CliOption("--start-window")]
    public string? StartWindow { get; set; }

    [CliOption("--uses-task-dependencies")]
    public string? UsesTaskDependencies { get; set; }
}