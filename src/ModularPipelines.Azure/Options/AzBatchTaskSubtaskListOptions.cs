using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "task", "subtask", "list")]
public record AzBatchTaskSubtaskListOptions(
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

    [CommandSwitch("--select")]
    public string? Select { get; set; }
}