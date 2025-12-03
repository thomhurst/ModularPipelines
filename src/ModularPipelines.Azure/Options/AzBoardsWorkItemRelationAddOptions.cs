using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("boards", "work-item", "relation", "add")]
public record AzBoardsWorkItemRelationAddOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--relation-type")] string RelationType
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--target-id")]
    public string? TargetId { get; set; }

    [CliOption("--target-url")]
    public string? TargetUrl { get; set; }
}