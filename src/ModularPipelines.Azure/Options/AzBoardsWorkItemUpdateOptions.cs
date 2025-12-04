using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("boards", "work-item", "update")]
public record AzBoardsWorkItemUpdateOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliOption("--area")]
    public string? Area { get; set; }

    [CliOption("--assigned-to")]
    public string? AssignedTo { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--discussion")]
    public string? Discussion { get; set; }

    [CliOption("--fields")]
    public string? Fields { get; set; }

    [CliOption("--iteration")]
    public string? Iteration { get; set; }

    [CliFlag("--open")]
    public bool? Open { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }
}