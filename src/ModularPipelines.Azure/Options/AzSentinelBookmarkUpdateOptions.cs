using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "bookmark", "update")]
public record AzSentinelBookmarkUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--bookmark-id")]
    public string? BookmarkId { get; set; }

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

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

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

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tactics")]
    public string? Tactics { get; set; }

    [CommandSwitch("--techniques")]
    public string? Techniques { get; set; }

    [CommandSwitch("--updated")]
    public string? Updated { get; set; }

    [CommandSwitch("--updated-by")]
    public string? UpdatedBy { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}