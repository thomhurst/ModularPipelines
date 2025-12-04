using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("boards", "area", "project", "update")]
public record AzBoardsAreaProjectUpdateOptions(
[property: CliOption("--path")] string Path
) : AzOptions
{
    [CliOption("--child-id")]
    public string? ChildId { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}