using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "trigger-run", "query-by-workspace")]
public record AzSynapseTriggerRunQueryByWorkspaceOptions(
[property: CommandSwitch("--last-updated-after")] string LastUpdatedAfter,
[property: CommandSwitch("--last-updated-before")] string LastUpdatedBefore,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--continuation-token")]
    public string? ContinuationToken { get; set; }

    [CommandSwitch("--filters")]
    public string? Filters { get; set; }

    [CommandSwitch("--order-by")]
    public string? OrderBy { get; set; }
}

