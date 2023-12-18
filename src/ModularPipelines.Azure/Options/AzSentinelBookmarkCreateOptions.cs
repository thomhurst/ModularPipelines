using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "bookmark", "create")]
public record AzSentinelBookmarkCreateOptions(
[property: CommandSwitch("--bookmark-id")] string BookmarkId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--created")]
    public string? Created { get; set; }

    [CommandSwitch("--created-by")]
    public string? CreatedBy { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--entity-mappings")]
    public string? EntityMappings { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--event-time")]
    public string? EventTime { get; set; }

    [CommandSwitch("--incident-info")]
    public string? IncidentInfo { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--notes")]
    public string? Notes { get; set; }

    [CommandSwitch("--query-content")]
    public string? QueryContent { get; set; }

    [CommandSwitch("--query-end-time")]
    public string? QueryEndTime { get; set; }

    [CommandSwitch("--query-result")]
    public string? QueryResult { get; set; }

    [CommandSwitch("--query-start-time")]
    public string? QueryStartTime { get; set; }

    [CommandSwitch("--tactics")]
    public string? Tactics { get; set; }

    [CommandSwitch("--techniques")]
    public string? Techniques { get; set; }

    [CommandSwitch("--updated")]
    public string? Updated { get; set; }

    [CommandSwitch("--updated-by")]
    public string? UpdatedBy { get; set; }
}

