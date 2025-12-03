using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "task", "file", "download")]
public record AzBatchTaskFileDownloadOptions(
[property: CliOption("--destination")] string Destination,
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

    [CliOption("--end-range")]
    public string? EndRange { get; set; }

    [CliOption("--if-modified-since")]
    public string? IfModifiedSince { get; set; }

    [CliOption("--if-unmodified-since")]
    public string? IfUnmodifiedSince { get; set; }

    [CliOption("--start-range")]
    public string? StartRange { get; set; }
}