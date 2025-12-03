using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "task", "file", "delete")]
public record AzBatchTaskFileDeleteOptions(
[property: CliOption("--file-path")] string FilePath,
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

    [CliOption("--recursive")]
    public string? Recursive { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}