using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "bookmark", "relation", "create")]
public record AzSentinelBookmarkRelationCreateOptions(
[property: CliOption("--bookmark-id")] string BookmarkId,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--related-resource-id")]
    public string? RelatedResourceId { get; set; }
}