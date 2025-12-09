using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "bookmark", "update")]
public record AzSentinelBookmarkUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--bookmark-id")]
    public string? BookmarkId { get; set; }

    [CliOption("--created")]
    public string? Created { get; set; }

    [CliOption("--created-by")]
    public string? CreatedBy { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--entity-mappings")]
    public string? EntityMappings { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--event-time")]
    public string? EventTime { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--incident-info")]
    public string? IncidentInfo { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--notes")]
    public string? Notes { get; set; }

    [CliOption("--query-content")]
    public string? QueryContent { get; set; }

    [CliOption("--query-end-time")]
    public string? QueryEndTime { get; set; }

    [CliOption("--query-result")]
    public string? QueryResult { get; set; }

    [CliOption("--query-start-time")]
    public string? QueryStartTime { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tactics")]
    public string? Tactics { get; set; }

    [CliOption("--techniques")]
    public string? Techniques { get; set; }

    [CliOption("--updated")]
    public string? Updated { get; set; }

    [CliOption("--updated-by")]
    public string? UpdatedBy { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}