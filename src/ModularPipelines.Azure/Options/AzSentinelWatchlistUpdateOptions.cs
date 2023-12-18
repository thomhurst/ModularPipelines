using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "watchlist", "update")]
public record AzSentinelWatchlistUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

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

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--is-deleted")]
    public bool? IsDeleted { get; set; }

    [CommandSwitch("--items-search-key")]
    public string? ItemsSearchKey { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--provider")]
    public string? Provider { get; set; }

    [CommandSwitch("--raw-content")]
    public string? RawContent { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--skip-num")]
    public string? SkipNum { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--source-type")]
    public string? SourceType { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

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

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}