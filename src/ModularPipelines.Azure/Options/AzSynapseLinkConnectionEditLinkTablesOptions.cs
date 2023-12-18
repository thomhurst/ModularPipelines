using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "link-connection", "edit-link-tables")]
public record AzSynapseLinkConnectionEditLinkTablesOptions(
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--continuation-token")]
    public string? ContinuationToken { get; set; }

    [CommandSwitch("--max-segment-count")]
    public int? MaxSegmentCount { get; set; }
}

