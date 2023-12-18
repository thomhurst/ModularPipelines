using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "pipeline-run", "query-by-workspace")]
public record AzSynapsePipelineRunQueryByWorkspaceOptions(
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