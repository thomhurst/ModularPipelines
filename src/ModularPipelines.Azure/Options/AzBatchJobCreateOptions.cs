using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "job", "create")]
public record AzBatchJobCreateOptions(
[property: CommandSwitch("--job-id")] string JobId
) : AzOptions
{
    [CommandSwitch("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [BooleanCommandSwitch("--allow-task-preemption")]
    public bool? AllowTaskPreemption { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--job-manager-task-command-line")]
    public string? JobManagerTaskCommandLine { get; set; }

    [CommandSwitch("--job-manager-task-environment-settings")]
    public string? JobManagerTaskEnvironmentSettings { get; set; }

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

    [CommandSwitch("--pool-id")]
    public string? PoolId { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--required-slots")]
    public string? RequiredSlots { get; set; }

    [CommandSwitch("--uses-task-dependencies")]
    public string? UsesTaskDependencies { get; set; }
}

