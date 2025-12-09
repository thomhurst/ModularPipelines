using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("batch", "job", "reset")]
public record AzBatchJobResetOptions(
[property: CliOption("--job-id")] string JobId
) : AzOptions
{
    [CliOption("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliFlag("--allow-task-preemption")]
    public bool? AllowTaskPreemption { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-modified-since")]
    public string? IfModifiedSince { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--if-unmodified-since")]
    public string? IfUnmodifiedSince { get; set; }

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
}