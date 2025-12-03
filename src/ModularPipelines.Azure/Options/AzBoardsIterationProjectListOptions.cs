using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("boards", "iteration", "project", "list")]
public record AzBoardsIterationProjectListOptions : AzOptions
{
    [CliOption("--depth")]
    public string? Depth { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}