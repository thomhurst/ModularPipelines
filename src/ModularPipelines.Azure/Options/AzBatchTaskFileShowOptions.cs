using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "task", "file", "show")]
public record AzBatchTaskFileShowOptions(
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

    [CommandSwitch("--if-modified-since")]
    public string? IfModifiedSince { get; set; }

    [CommandSwitch("--if-unmodified-since")]
    public string? IfUnmodifiedSince { get; set; }
}

