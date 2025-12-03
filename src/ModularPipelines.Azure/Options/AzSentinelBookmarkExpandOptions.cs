using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sentinel", "bookmark", "expand")]
public record AzSentinelBookmarkExpandOptions(
[property: CliOption("--bookmark-id")] string BookmarkId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--expansion-id")]
    public string? ExpansionId { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }
}