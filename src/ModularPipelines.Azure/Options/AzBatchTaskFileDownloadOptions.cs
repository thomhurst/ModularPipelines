using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "task", "file", "download")]
public record AzBatchTaskFileDownloadOptions(
[property: CommandSwitch("--destination")] string Destination,
[property: CommandSwitch("--file-path")] string FilePath,
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--task-id")] string TaskId
) : AzOptions
{
    [CommandSwitch("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--end-range")]
    public string? EndRange { get; set; }

    [CommandSwitch("--if-modified-since")]
    public string? IfModifiedSince { get; set; }

    [CommandSwitch("--if-unmodified-since")]
    public string? IfUnmodifiedSince { get; set; }

    [CommandSwitch("--start-range")]
    public string? StartRange { get; set; }
}