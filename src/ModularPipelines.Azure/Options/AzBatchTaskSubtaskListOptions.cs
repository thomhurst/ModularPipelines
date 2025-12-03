using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "task", "subtask", "list")]
public record AzBatchTaskSubtaskListOptions(
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

    [CliOption("--select")]
    public string? Select { get; set; }
}