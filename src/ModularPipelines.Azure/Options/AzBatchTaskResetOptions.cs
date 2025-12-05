using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("batch", "task", "reset")]
public record AzBatchTaskResetOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--task-id")] string TaskId
) : AzOptions
{
    [CliOption("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-modified-since")]
    public string? IfModifiedSince { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--if-unmodified-since")]
    public string? IfUnmodifiedSince { get; set; }

    [CliOption("--json-file")]
    public string? JsonFile { get; set; }

    [CliOption("--max-task-retry-count")]
    public int? MaxTaskRetryCount { get; set; }

    [CliOption("--max-wall-clock-time")]
    public string? MaxWallClockTime { get; set; }

    [CliOption("--retention-time")]
    public string? RetentionTime { get; set; }
}