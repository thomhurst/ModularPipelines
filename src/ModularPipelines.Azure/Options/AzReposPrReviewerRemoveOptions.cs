using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repos", "pr", "reviewer", "remove")]
public record AzReposPrReviewerRemoveOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--reviewers")] string Reviewers
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }
}