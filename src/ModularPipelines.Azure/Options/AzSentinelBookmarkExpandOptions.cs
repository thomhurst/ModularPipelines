using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "bookmark", "expand")]
public record AzSentinelBookmarkExpandOptions(
[property: CommandSwitch("--bookmark-id")] string BookmarkId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--expansion-id")]
    public string? ExpansionId { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }
}