using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("boards", "work-item", "create")]
public record AzBoardsWorkItemCreateOptions(
[property: CliOption("--title")] string Title,
[property: CliOption("--type")] string Type
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

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--reason")]
    public string? Reason { get; set; }
}