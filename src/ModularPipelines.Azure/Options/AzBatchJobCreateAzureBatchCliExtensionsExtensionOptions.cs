using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("batch", "job", "create", "(ure-batch-cli-extensions", "extension)")]
public record AzBatchJobCreateAzureBatchCliExtensionsExtensionOptions : AzOptions
{
    [CliOption("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliFlag("--allow-task-preemption")]
    public bool? AllowTaskPreemption { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--job-manager-task-command-line")]
    public string? JobManagerTaskCommandLine { get; set; }

    [CliOption("--job-manager-task-environment-settings")]
    public string? JobManagerTaskEnvironmentSettings { get; set; }

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

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--pool-id")]
    public string? PoolId { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--required-slots")]
    public string? RequiredSlots { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }

    [CliFlag("--uses-task-dependencies")]
    public bool? UsesTaskDependencies { get; set; }
}