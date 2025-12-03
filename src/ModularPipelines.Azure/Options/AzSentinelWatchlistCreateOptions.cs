using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sentinel", "watchlist", "create")]
public record AzSentinelWatchlistCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--created")]
    public string? Created { get; set; }

    [CliOption("--created-by")]
    public string? CreatedBy { get; set; }

    [CliOption("--default-duration")]
    public string? DefaultDuration { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliFlag("--is-deleted")]
    public bool? IsDeleted { get; set; }

    [CliOption("--items-search-key")]
    public string? ItemsSearchKey { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--provider")]
    public string? Provider { get; set; }

    [CliOption("--raw-content")]
    public string? RawContent { get; set; }

    [CliOption("--skip-num")]
    public string? SkipNum { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--source-type")]
    public string? SourceType { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }

    [CliOption("--updated")]
    public string? Updated { get; set; }

    [CliOption("--updated-by")]
    public string? UpdatedBy { get; set; }

    [CliOption("--upload-status")]
    public string? UploadStatus { get; set; }

    [CliOption("--watchlist-id")]
    public string? WatchlistId { get; set; }

    [CliOption("--watchlist-type")]
    public string? WatchlistType { get; set; }
}