using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "watchlist", "create")]
public record AzSentinelWatchlistCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--created")]
    public string? Created { get; set; }

    [CommandSwitch("--created-by")]
    public string? CreatedBy { get; set; }

    [CommandSwitch("--default-duration")]
    public string? DefaultDuration { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [BooleanCommandSwitch("--is-deleted")]
    public bool? IsDeleted { get; set; }

    [CommandSwitch("--items-search-key")]
    public string? ItemsSearchKey { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--provider")]
    public string? Provider { get; set; }

    [CommandSwitch("--raw-content")]
    public string? RawContent { get; set; }

    [CommandSwitch("--skip-num")]
    public string? SkipNum { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--source-type")]
    public string? SourceType { get; set; }

    [CommandSwitch("--tenant-id")]
    public string? TenantId { get; set; }

    [CommandSwitch("--updated")]
    public string? Updated { get; set; }

    [CommandSwitch("--updated-by")]
    public string? UpdatedBy { get; set; }

    [CommandSwitch("--upload-status")]
    public string? UploadStatus { get; set; }

    [CommandSwitch("--watchlist-id")]
    public string? WatchlistId { get; set; }

    [CommandSwitch("--watchlist-type")]
    public string? WatchlistType { get; set; }
}