using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sentinel", "bookmark", "create")]
public record AzSentinelBookmarkCreateOptions(
[property: CliOption("--bookmark-id")] string BookmarkId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
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

    [CliOption("--tactics")]
    public string? Tactics { get; set; }

    [CliOption("--techniques")]
    public string? Techniques { get; set; }

    [CliOption("--updated")]
    public string? Updated { get; set; }

    [CliOption("--updated-by")]
    public string? UpdatedBy { get; set; }
}