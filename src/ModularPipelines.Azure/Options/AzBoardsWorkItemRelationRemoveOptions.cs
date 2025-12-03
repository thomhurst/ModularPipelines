using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("boards", "work-item", "relation", "remove")]
public record AzBoardsWorkItemRelationRemoveOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--relation-type")] string RelationType,
[property: CliOption("--target-id")] string TargetId
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}